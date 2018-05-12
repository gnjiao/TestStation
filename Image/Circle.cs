using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image
{
    public class Circle
    {
        public readonly int[][] PixelValues;

        public int[] RowMap;
        public int RowStart;
        public int RowEnd;

        public int[] ColMap;
        public int ColStart;
        public int ColEnd;

        public Circle(int[][] array)
        {
            PixelValues = array;
        }
        public static void Map(Circle circle)
        {
            circle.ColMap = new int[circle.PixelValues[0].Length];
            circle.RowMap = new int[circle.PixelValues.Length];

            for (int row = 0; row < circle.PixelValues.Length; row++)
            {
                for (int col = 0; col < circle.PixelValues.Length; col++)
                {
                    circle.ColMap[col] += circle.PixelValues[row][col];
                    circle.RowMap[row] += circle.PixelValues[row][col];

                    if (circle.PixelValues[row][col] > 0)
                    {
                        if (circle.ColStart == 0)
                        {
                            circle.ColStart = col;
                        }

                        if (circle.RowStart == 0)
                        {
                            circle.RowStart = row;
                        }

                        circle.ColEnd = col;
                        circle.RowEnd = col;
                    }
                }
            }
        }
    }
}
