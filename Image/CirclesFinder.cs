using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace JbImage
{
    public class CirclesFinder
    {
        private byte[][] _binArray;
        private Bitmap _rawImg;

        public List<Round> Rounds = new List<Round>();
        private List<Round> RunningRounds
        {
            get {
                return Rounds.FindAll(x => !x.IsEnd).ToList();
            }
        }

        public void Add(Round r)
        {
            r.Id = Rounds.Count + 1;
            Rounds.Add(r);
        }
        public CirclesFinder(string path)
        {
            _rawImg = (Bitmap)Bitmap.FromFile(path);
            _binArray = Preprocess.ToArray(_rawImg);
            Execute(_binArray);
        }
        public CirclesFinder(byte[][] binArray)
        {
            _binArray = binArray;
            Execute(_binArray);
        }
        public void Execute(byte[][] binArray)
        {
            for (int row = 0; row < binArray.Length; row++)
            {
                List<Line> lines = FindLines(binArray[row]);

                foreach (var round in RunningRounds)
                {
                    round.StartScan(row);
                }

                foreach (var line in lines)
                {
                    bool SeperatedLine = true;

                    foreach (var round in RunningRounds)
                    {
                        if (line.AddTo(round))
                        {
                            SeperatedLine = false;
                            break;/* a line will only add to one round */
                        }
                    }

                    if (SeperatedLine)
                    {
                        Round newRound = new Round();

                        newRound.StartScan(row);
                        newRound.StartY = row;

                        line.AddTo(newRound);
                        Add(newRound);
                    }
                }

                foreach (var round in RunningRounds)
                {
                    round.FinishScan();
                }
            }
        }
        public void Draw(string path)
        {
            Bitmap b = _rawImg!=null ? _rawImg : new Bitmap(_binArray[0].Length, _binArray.Length);
            Graphics g = Graphics.FromImage(b);

            foreach (var r in Rounds)
            {
                g.DrawEllipse(new Pen(Color.Red), r.ImgLeftTopX, r.ImgLeftTopY, r.ImgX, r.ImgY);
            }
            b.Save(path);
        }
        public static List<Line> FindLines(byte[] binArray)
        {
            List<Line> lines = new List<Line>();

            Line newLine = null;
            for (int i = 0; i < binArray.Length; i++)
            {
                if (binArray[i] > 0)/* white */
                {
                    if (newLine == null)
                    {
                        newLine = new Line(i);
                    }
                    else
                    {
                        newLine.End = i;
                    }
                }
                else
                {
                    if (newLine != null)
                    {
                        newLine.End = i - 1;
                        lines.Add(newLine);
                        newLine = null;
                    }
                }

            }

            if (newLine != null)
            {
                newLine.End = binArray.Length - 1;
                lines.Add(newLine);
            }

            return lines;
        }
    }
}