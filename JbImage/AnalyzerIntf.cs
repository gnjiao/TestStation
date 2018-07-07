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
        public static int CountPixels(Image<Gray, Byte> img, CircleF circle)
        {
            double centerX = circle.Center.X;
            double centerY = circle.Center.Y;
            double r = circle.Radius;
            int sum = 0;

            for (int x = (int)System.Math.Floor(centerX - r); x < (int)System.Math.Ceiling(centerX + r); x++)
            {
                for (int y = (int)System.Math.Floor(centerY - r); y < (int)System.Math.Ceiling(centerY + r); y++)
                {
                    sum += img.Data[y, x, 0];
                }
            }

            return sum;
        }
    }
}