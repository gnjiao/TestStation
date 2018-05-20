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
        #region initialize field
        private byte[][] _binArray;
        private Bitmap _rawImg;
        #endregion

        public List<Round> Rounds = new List<Round>();
        private List<Round> RunningRounds
        {
            get {
                return Rounds.FindAll(x => !x.IsEnd).ToList();
            }
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
                List<Line> lines = Line.FindLines(binArray[row]);

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
                g.DrawString(r.Id.ToString(),new Font("黑体", 25, FontStyle.Regular), new SolidBrush(Color.Red),
                    TextPoint(r.Id, r.ImgLeftTopX, r.ImgLeftTopY, r.ImgX));
            }
            b.Save(path);
        }
        private PointF TextPoint(int num, int x, int y, int r)
        {
            if (num < 10)
            {
                return new PointF((float)(x + r / 4), (float)(y + r / 4));
            }
            else if (num < 100)
            {
                return new PointF((float)(x + r / 8), (float)(y + r / 4));
            }
            else
            {
                return new PointF((float)(x), (float)(y + r / 8));
            }
        }

        public void Add(Round r)
        {
            r.Id = Rounds.Count + 1;
            Rounds.Add(r);
        }
    }
}