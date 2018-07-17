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
    public abstract class AnalyzerIntf
    {
        public static int CfgMinRadiusFor863 = 10;
        public static AnalyzerIntf GetAnalyzer(string testType)
        {
            switch (testType)
            {
                case "NFT":
                    return new AnalyzerNFT();
                case "FFT":
                    return new AnalyzerFFT();
            }

            return null;
        }

        public virtual CircleImage FindCircle(Image<Gray, ushort> image, Parameters param = null)
        {
            throw new Exception("FindCircle on image is not implemented yet");
        }
        public abstract CircleImage FindCircle(string path, Parameters param = null);
        public abstract Result Calculate(List<CircleImage> img, List<double> distance);

        protected Logger _log = new Logger("Analyzer");
        public string Path;
        public Image<Bgr, Byte> RawImg;

        public CircleF[] Circles;
        public CircleF[] Circles2nd;
        public List<CircleF> FilteredCircles;
        public List<CircleF> FilteredCircles2nd;
        public List<int> FilteredLights;

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
        public static int CountPixels(Image<Gray, Byte> img, ref CircleF circle)
        {
            CountXPixels(img, circle);

            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            for (int x = (int)System.Math.Floor(centerX - r); x < (int)System.Math.Ceiling(centerX + r); x++)
            {
                int[] rangeY = RangeY(circle, x);
                for (int y = rangeY[0]; y <= rangeY[1]; y++)
                {
                    sum += img.Data[y, x, 0];
                }
            }

            double prevRatio = 0;
            for (int radius = CfgMinRadiusFor863; radius < circle.Radius; radius++)
            {
                int rsum = 0;

                for (int x = (int)System.Math.Floor(centerX - radius); x < (int)System.Math.Ceiling(centerX + radius); x++)
                {
                    int[] rangeY = RangeY(circle, x);
                    for (int y = rangeY[0]; y <= rangeY[1]; y++)
                    {
                        rsum += img.Data[y, x, 0];
                    }
                }

                double ratio = (double)rsum / sum;
                new Logger("Analyzer").Debug($"radius({radius}) rsum({rsum}) sum({sum}) ratio {(double)rsum / sum:F3}");

                if (ratio < 0.863)
                {
                    prevRatio = ratio;
                }
                else
                {
                    circle.Radius = radius;
                    sum = rsum;
                    break;
                }
            }

            return sum;
        }

        public static void CountXPixels(Image<Gray, byte> img, CircleF circle)
        {
            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            for (int x = (int)System.Math.Floor(centerX - r); x <= (int)System.Math.Ceiling(centerX); x++)
            {
                sum += img.Data[(int)System.Math.Ceiling(centerY), x, 0];
            }

            for (int radius = 10; radius <= circle.Radius; radius++)
            {
                int rsum = 0;

                for (int x = (int)System.Math.Floor(centerX - radius); x <= (int)System.Math.Ceiling(centerX); x++)
                {
                    rsum += img.Data[(int)System.Math.Ceiling(centerY), x, 0];
                }

                new Logger("Analyzer").Debug($"r {radius} rXsum {rsum} Xsum {sum} ratio {(double)rsum / sum:F3}");
                if ((double)rsum / sum > 0.863)
                {
                    break;
                }
            }
        }
        public static int CountPixelsSquare(Image<Gray, Byte> img, CircleF circle)
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
        public static int CountPixels(Image<Gray, ushort> img, CircleF circle)
        {
            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            for (int x = (int)System.Math.Floor(centerX - r); x <= (int)System.Math.Ceiling(centerX + r); x++)
            {
                int[] rangeY = RangeY(circle, x);
                for (int y = rangeY[0]; y <= rangeY[1]; y++)
                {
                    sum += img.Data[y, x, 0];
                }
            }

            return sum;
        }

        private static int[] RangeY(CircleF circle, int x)
        {
            double h = System.Math.Sqrt(circle.Radius * circle.Radius - (circle.Center.X - x) * (circle.Center.X - x));
            if (circle.Center.X - x > circle.Radius)
            {
                h = 0;
            }
            return new int[] {
                (int)System.Math.Floor(circle.Center.Y - h),
                (int)System.Math.Ceiling(circle.Center.Y + h)
            };
        }
    }
}