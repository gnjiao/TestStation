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
        public static int CfgMinRadiusFor865 = 10;
        public static double ValidRatio
        {
            get
            {
                if (CfgMinRadiusFor865 == 1)
                {
                    return 1;
                }
                else
                {
                    return 0.865;
                }
            }
        }
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
        private static int SumCircelPixel(Image<Gray, Byte> img, CircleF circle)
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
        public static int CountPixels(Image<Gray, Byte> img, ref CircleF circle, double targetRatio)
        {
            //CountXPixels(img, circle, targetRatio);

            List<int> Radius = new List<int>();
            List<int> SumOnRadius = new List<int>();

            int sum = SumCircelPixel(img, circle);

            double prevRatio = 0;
            int prevSum = 0;
            for (int radius = CfgMinRadiusFor865 - 1; radius <= System.Math.Ceiling(circle.Radius); radius++)
            {
                int rsum = SumCircelPixel(img, new CircleF(circle.Center, radius));

                Radius.Add(radius);
                SumOnRadius.Add(rsum);

                double ratio = (double)rsum / sum;
                if (ratio < targetRatio)
                {
                    prevRatio = ratio;
                    prevSum = rsum;
                    continue;
                }

                #region linear calculation
                circle.Radius = (float)((radius - 1) + (targetRatio - prevRatio)/(ratio - prevRatio));
                sum = (int)(prevSum + (targetRatio - prevRatio) / (ratio - prevRatio));
                #endregion
                break;
            }

            return sum;
        }

        public static void CountXPixels(Image<Gray, byte> img, CircleF circle, double targetRatio)
        {
            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            #region point values(center -> left)
            List<int> values = new List<int>();
            for (int x = (int)System.Math.Ceiling(centerX); x >= (int)System.Math.Floor(centerX - r); x--)
            {
                values.Add( img.Data[(int)System.Math.Ceiling(centerY), x, 0] );
            }
            new Logger("Analyzer").Debug($"CountPixels(point):  [{Utils.String.FromList<int>(values)}]';");
            #endregion

            for (int x = (int)System.Math.Floor(centerX - r); x <= (int)System.Math.Ceiling(centerX); x++)
            {
                sum += img.Data[(int)System.Math.Ceiling(centerY), x, 0];
            }

            List<int> SumOnRadius = new List<int>();
            for (int radius = CfgMinRadiusFor865 - 1; radius <= circle.Radius; radius++)
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