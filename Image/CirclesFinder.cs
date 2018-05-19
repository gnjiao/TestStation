using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace JbImage
{
    public class Line
    {
        public int Start;
        public int End;
        public int Length
        {
            get
            {
                return (End - Start + 1);
            }
        }
        public Line(int start)
        {
            Start = start;
            End = start;
        }
        public Line(int start, int end)
        {
            Start = start;
            End = end;
        }
        public bool Equals(Line l)
        {
            return (Start == l.Start && End == l.End);
        }
        public bool LongerThan(Line l)
        {
            return Length > l.Length;
        }
        public bool AdjacentTo(Line line)
        {
            return !((End < line.Start) || (Start > line.End));
        }
        public void Merge(Line line)
        {
            End = line.End;
        }
        public bool AddTo(Round round)
        {
            if (round.Bottom == null || (!round.IsLineAdded && AdjacentTo(round.Bottom))
                || (round.IsLineAdded && (round.Lines.Count - 2) >= 0 && AdjacentTo(round.Lines[round.Lines.Count - 2])))
            {
                round.Add(this);
                return true;
            }

            return false;
        }
    }
    public class Round
    {
        public int Id;
        public List<Line> Lines = new List<Line>();
        public void Add(Line l)
        {
            EndY = _rowNo;

            if (!IsLineAdded)
            {
                Lines.Add(l);
                IsLineAdded = true;
            }
            else
            {
                Bottom.Merge(l);
            }

            if (MaxLenLine == null || l.LongerThan(MaxLenLine))
            {
                MaxLenLine = l;
                MaxLenY = _rowNo;
            }

            IsEnd = false;
        }
        public void StartScan(int rowNo)
        {
            _rowNo = rowNo;
            IsLineAdded = false;
        }
        public void FinishScan()
        {
            IsEnd = !IsLineAdded;
        }
        #region image information
        public int LeftTopX
        {
            get {
                return MaxLenLine.Start;
            }
        }
        public int LeftTopY
        {
            get {
                return StartY;
            }
        }
        public int X
        {
            get {
                return MaxLenLine.Length;
            }
        }
        public int Y
        {
            get {
                return (EndY - StartY);
            }
        }
        #endregion
        #region statistic information
        public int StartY;
        public int EndY;
        public int MaxLenY;
        public Line MaxLenLine;
        #endregion
        #region running information
        private int _rowNo;
        public Line Bottom
        {
            get
            {
                if (Lines.Count > 0)
                {
                    return Lines[Lines.Count - 1];
                }

                return null;
            }
        }
        public bool IsLineAdded;
        public bool IsEnd;
        #endregion
    }
    public class CirclesFinder
    {
        private byte[][] _binArray;

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
        public CirclesFinder(byte[][] binArray)
        {
            _binArray = binArray;

            for (int row = 0; row<binArray.Length; row++)
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
                        Rounds.Add(newRound);
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
            Bitmap b = new Bitmap(_binArray[0].Length, _binArray.Length);
            Graphics g = Graphics.FromImage(b);

            foreach (var r in Rounds)
            {
                g.DrawEllipse(new Pen(Color.Red), r.LeftTopX, r.LeftTopY, r.X, r.Y);
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