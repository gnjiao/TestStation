using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Image
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
        public bool AddTo(Round round)
        {
            if (round.Bottom == null || AdjacentTo(round.Bottom))
            {
                round.Add(this);
                return true;
            }

            return false;
        }
    }
    public class Round
    {
        public List<Line> Lines = new List<Line>();
        public void Add(Line l)
        {
            EndY = _rowNo;

            Lines.Add(l);

            if (MaxLenLine == null || l.LongerThan(MaxLenLine))
            {
                MaxLenLine = l;
                MaxLenY = _rowNo;
            }

            IsLineAdded = true;
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
        private int[][] _binArray;

        public List<Round> Rounds = new List<Round>();
        private List<Round> RunningRounds
        {
            get {
                return Rounds.FindAll(x => !x.IsEnd).ToList();
            }
        }

        public CirclesFinder(int[][] binArray)
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

        public static List<Line> FindLines(int[] binArray)
        {
            List<Line> lines = new List<Line>();

            Line newLine = null;
            for (int i = 0; i < binArray.Length; i++)
            {
                if (binArray[i] == 1)
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