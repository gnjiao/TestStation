using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

namespace JbImage
{
    public class PixelCount
    {
        public static int Circle(Image<Gray, Byte> img, CircleF circle)
        {
            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            for (int x = (int)System.Math.Floor(centerX - r); x <= (int)System.Math.Ceiling(centerX + r); x++)
            {
                int[] rangeY = Calc.RangeY(circle, x);
                for (int y = rangeY[0]; y <= rangeY[1]; y++)
                {
                    sum += img.Data[y, x, 0];
                }
            }

            return sum;
        }
        public static void CircleX(Image<Gray, byte> img, CircleF circle, double targetRatio)
        {
            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            #region point values(center -> left)
            List<int> values = new List<int>();
            for (int x = (int)System.Math.Ceiling(centerX); x >= (int)System.Math.Floor(centerX - r); x--)
            {
                values.Add(img.Data[(int)System.Math.Ceiling(centerY), x, 0]);
            }
            new Logger("Analyzer").Debug($"CountPixels(point):  [{Utils.String.FromList<int>(values)}]';");
            #endregion

            for (int x = (int)System.Math.Floor(centerX - r); x <= (int)System.Math.Ceiling(centerX); x++)
            {
                sum += img.Data[(int)System.Math.Ceiling(centerY), x, 0];
            }

            List<int> SumOnRadius = new List<int>();
            for (int radius = AnalyzerIntf.CfgMinRadiusFor865 - 1; radius <= circle.Radius; radius++)
            {
                int rsum = 0;

                for (int x = (int)System.Math.Floor(centerX - radius); x <= (int)System.Math.Ceiling(centerX); x++)
                {
                    rsum += img.Data[(int)System.Math.Ceiling(centerY), x, 0];
                }
                SumOnRadius.Add(rsum);

                //new Logger("Analyzer").Debug($"r {radius} rXsum {rsum} Xsum {sum} ratio {(double)rsum / sum:F3}");
                if ((double)rsum / sum > targetRatio)
                {
                    break;
                }
            }
            new Logger("Analyzer").Debug($"CountPixels(Xsum): [{Utils.String.FromList<int>(SumOnRadius)}]';");
        }
        public static int Square(Image<Gray, Byte> img, CircleF circle)
        {
            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            for (int x = (int)System.Math.Floor(centerX - r); x <= (int)System.Math.Ceiling(centerX + r); x++)
            {
                for (int y = (int)System.Math.Floor(centerY - r); y <= (int)System.Math.Ceiling(centerY + r); y++)
                {
                    sum += img.Data[y, x, 0];
                }
            }

            return sum;
        }
    }
    public class Calc
    {
        public static CircleF[] Sort(CircleF[] circles)
        {
            List<CircleF> temp = circles.ToList();
            temp.Sort((c1, c2) => {
                if (c1.Center.Y < c2.Center.Y)
                {
                    return -1;
                }
                else if (c1.Center.Y == c2.Center.Y && c1.Center.X < c2.Center.X)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
            return temp.ToArray();
        }
        public static int[] RangeY(CircleF circle, int x)
        {
            double h = 0;
            if (System.Math.Abs(circle.Center.X - x) < circle.Radius)
            {
                h = System.Math.Sqrt(circle.Radius * circle.Radius - (circle.Center.X - x) * (circle.Center.X - x));
            }

            return new int[] {
                (int)System.Math.Floor(circle.Center.Y - h),
                (int)System.Math.Ceiling(circle.Center.Y + h)
            };
        }
    }
}