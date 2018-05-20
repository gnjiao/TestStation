﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

namespace JbImage
{
    public class CirclesFinder
    {
        #region initialize field
        private byte[][] _binArray;
        private Bitmap _rawImg;
        public int Filter = 0;                /* filter rounds whose radius less than Filter */
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
            Filter = 5;
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
#if false
            _logger.Debug("Bitmap array:");
            _logger.Debug(Utils.Array.ToString<byte>(result));
#endif
            for (int row = 0; row < binArray.Length; row++)
            {
                List<Line> lines = Line.FindLines(binArray[row]);
                foreach (var line in lines)
                {
                    line.Y = row;
                }

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

                #region filter rounds cuased by unsmooth
                for (int i = Rounds.Count - 1; i >= 0; i--)
                {
                    if (Rounds[i].IsEnd && Rounds[i].Lines.Count < Filter/*this should be minimum possible radius of the round*/)
                    {
                        Rounds.RemoveAt(i);
                    }
                }
                #endregion
            }
            /* reassign id */
            for (int i = 0; i < Rounds.Count; i++)
            {
                Rounds[i].Id = i + 1;
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
#if false
                Logger log = new Logger();
                log.Debug(r.ToString());
#endif
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
            Rounds.Add(r);
        }
    }
}