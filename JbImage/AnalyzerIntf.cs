using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
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

        public static int CountPixels(Image<Gray, Byte> img, ref CircleF circle, double targetRatio)
        {
            //Calc.CircleX(img, circle, targetRatio);

            List<int> Radius = new List<int>();
            List<int> SumOnRadius = new List<int>();

            int sum = PixelCount.Circle(img, circle);

            double prevRatio = 0;
            int prevSum = 0;
            for (int radius = CfgMinRadiusFor865 - 1; radius <= System.Math.Ceiling(circle.Radius); radius++)
            {
                int rsum = PixelCount.Circle(img, new CircleF(circle.Center, radius));

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

        public static int CountPixels(Image<Gray, ushort> img, CircleF circle)
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

    }
}