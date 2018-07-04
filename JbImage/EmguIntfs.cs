using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Utils;

namespace JbImage
{
    public class EmguIntfs
    {
        //basic transform
        public static UMat ToUMat(Image<Gray, Byte> image)
        {
            return image.ToUMat();
        }
        public static Image<Gray, Byte> ToImage(UMat uimage)
        {
            return uimage.ToImage<Gray, Byte>();
        }
        //image functions
        public static Image<Bgr, Byte> Load(string path)
        {
            return new Image<Bgr, Byte>(path);
        }
        public static Image<Bgr, Byte> Resize(Image<Bgr, Byte> image, int x, int y)
        {
            return image.Resize(x, y, Emgu.CV.CvEnum.Inter.Linear, true);
        }
        public static UMat Grayed(Image<Bgr, Byte> image)
        {
            UMat uimage = new UMat();
            CvInvoke.CvtColor(image, uimage, ColorConversion.Bgr2Gray);
            return uimage;
        }
        public static Image<Gray, Byte> Canny(Image<Gray, Byte> uimage, double d1 = 90, double d2 = 180, int i1 = 3, bool b1 = false)
        {
            Image<Gray, Byte> edge = new Image<Gray, Byte>(uimage.Cols, uimage.Rows);
            CvInvoke.Canny(uimage, edge, d1, d2, i1, b1);
            return edge;
        }
        public static Image<Gray, Byte> PyrRemoveNoise(Image<Gray, Byte> uimage)
        {
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);
            return uimage;
        }
        public static Image<Gray, float> Sharpen(Image<Gray, Byte> image)
        {
            float[,] k = { {0, 1, 0},
                        {1, -4, 1},
                        {0, 1, 0}};
            ConvolutionKernelF kernel = new ConvolutionKernelF(k);

            image.SetRandUniform(new MCvScalar(0.0), new MCvScalar(255.0));
            Image<Gray, float> laplace = image.Laplace(1);
            Image<Gray, float> output = image * kernel;

            return output;
        }
        public static Image<Gray, Byte> BilateralFilter(Image<Gray, Byte> grayImage)
        {
            var threshImage = grayImage.CopyBlank();
            CvInvoke.BilateralFilter(grayImage, threshImage, 25, 25 * 2, 25 / 2);
            return threshImage;
        }
        public static Image<Gray, Byte> Binarize(int threshold, Image<Gray, Byte> grayImage)
        {
            var threshImage = grayImage.CopyBlank();
            CvInvoke.Threshold(grayImage, threshImage, threshold, 255, ThresholdType.Binary);
            return threshImage;
        }
        /*jiangbo : bug here, may count pixel outside the circle*/
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
        const string testImage = @"D:\work\TestStation\ImageTool\Samples\Sample3.jpg";
        const string resultPath = @"D:\";
        public static void TestBinarize()
        {
            string testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            StringBuilder msgBuilder = new StringBuilder("Performance: ");

            Stopwatch watch = Stopwatch.StartNew();
            Image<Bgr, Byte> img = Load(testImage);
            UMat uimage = Grayed(img);

            Image<Gray, Byte> image = Binarize(30, ToImage(uimage));
            CircleF[] circles = CvInvoke.HoughCircles(image, HoughType.Gradient, 2, 40, 180, 13, 18, 20);

            watch.Stop();
            msgBuilder.Append(string.Format("{0} Hough circles - {1} ms; ", testName, watch.ElapsedMilliseconds));

            Mat circleImage = img.Mat;
            for(int i = 0; i<circles.Length; i++)
            {
                CvInvoke.Circle(circleImage, Point.Round(circles[i].Center), (int)circles[i].Radius, new Bgr(Color.Red).MCvScalar, 2);
                CvInvoke.PutText(circleImage, i.ToString("D3"), new Point((int)circles[i].Center.X, (int)circles[i].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
            }
            circleImage.Save(resultPath + testName + ".jpg");

            (new Logger()).Debug(msgBuilder.ToString());

            msgBuilder = new StringBuilder("Circles: " + Environment.NewLine);
            int[] lights = new int[circles.Length];
            for (int i = 0; i < circles.Length; i++)
            {
                lights[i] = CountPixels(ToImage(uimage), circles[i]);
                msgBuilder.Append(string.Format("{0} {1}", i.ToString("D3"), lights[i]) + Environment.NewLine);
            }
            (new Logger()).Debug(msgBuilder.ToString());
        }
        public static void Test()
        {
            Image<Bgr, Byte> img = Load(testImage);
            UMat uimage = Grayed(img);

            Mat mask = new Mat(uimage.Size, DepthType.Cv8U, 1);
            CvInvoke.Circle(mask, new Point(10, 10), 10, new Bgr(Color.Black).MCvScalar, -1);

            Mat imgPart = new Mat();
        }
    }
    public class EmguCircleImage
    {
        private Logger _log = new Logger(typeof(EmguCircleImage));
        private string _imgPath;
        private Image<Bgr, Byte> _rawImg;
        private UMat _grayedUmat;
        public CircleF[] Circles;
        public CircleF[] Circles2nd;
        public int[] Lights;
        public List<CircleF> FilteredCircles;
        public List<CircleF> FilteredCircles2nd;
        public List<int> FilteredLights;

        private void NFTAnalyze(string path)
        {
            _log.Debug(Utils.String.Flatten(EmguParameters.Item));

            bool saveFile = bool.Parse(EmguParameters.Item["SaveFile"]);
            bool useCanny = bool.Parse(EmguParameters.Item["UseCanny"]);

            string testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            StringBuilder msgBuilder = new StringBuilder("Performance: ");

            Stopwatch watch = Stopwatch.StartNew();
            _imgPath = path;
            _rawImg = EmguIntfs.Load(path);
            _grayedUmat = EmguIntfs.Grayed(_rawImg);

            Image<Gray, Byte> _edged;

            if (useCanny)
            {
                _edged = EmguIntfs.Canny(EmguIntfs.ToImage(_grayedUmat),
                    double.Parse(EmguParameters.Item["Canny1Threshold1"]),
                    double.Parse(EmguParameters.Item["Canny1Threshold2"]),
                    Int32.Parse(EmguParameters.Item["Canny1ApertureSize"]),
                    bool.Parse(EmguParameters.Item["Canny1I2Gradient"]));
                if (saveFile)
                {
                    _edged.Save(Utils.String.FilePostfix(_imgPath, "-1-edge"));
                }
            }
            else
            {
                _edged = EmguIntfs.ToImage(_grayedUmat);
            }

            Image<Gray, Byte> image = EmguIntfs.Binarize(Int32.Parse(EmguParameters.Item["BinThreshold"]), _edged);
            Circles = CvInvoke.HoughCircles(image, HoughType.Gradient,
                double.Parse(EmguParameters.Item["Hough1Dp"]),
                double.Parse(EmguParameters.Item["Hough1MinDist"]),
                double.Parse(EmguParameters.Item["Hough1Param1"]),
                double.Parse(EmguParameters.Item["Hough1Param2"]),
                Int32.Parse(EmguParameters.Item["Hough1MinRadius"]),
                Int32.Parse(EmguParameters.Item["Hough1MaxRadius"]));
            Circles = Sort(Circles);
            #region filter 86.3%
            FilteredCircles = new List<CircleF>();
            FilteredLights = new List<int>();
            var raw = EmguIntfs.ToImage(_grayedUmat);
            foreach (var circle in Circles)
            {
                int extra = Int32.Parse(EmguParameters.Item["FilterSizeExtra"]);

                int startX = (int)System.Math.Floor(circle.Center.X - circle.Radius - extra);
                int startY = (int)System.Math.Floor(circle.Center.Y - circle.Radius - extra);
                int len = (int)System.Math.Ceiling((double)circle.Radius * 2.0) + 2 * extra;
                if (startX < 0 || startY < 0)
                {
                    _log.Warn("FilterSizeExtra may be too big, filter abandoned");
                    continue;
                }

                int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
                if (strength < 30)
                {
                    continue;
                }
                else
                {
                    FilteredCircles.Add(circle);

                    int threshold = (int)((double)strength * 0.863);

                    raw.ROI = new Rectangle(startX, startY, len, len);

                    Image<Gray, Byte> oneCircle = EmguIntfs.Binarize(threshold, raw);
                    raw.ROI = Rectangle.Empty;

                    for (int x = 0; x < len; x++)
                    {
                        for (int y = 0; y < len; y++)
                        {
                            raw.Data[startY + y, startX + x, 0] = oneCircle.Data[y, x, 0];
                        }
                    }
                }
            }
            if (saveFile)
            {
                raw.Save(Utils.String.FilePostfix(_imgPath, "-2-filter"));
            }
            #endregion

            if (useCanny)
            {
                _edged = EmguIntfs.Canny(raw,
                    double.Parse(EmguParameters.Item["Canny2Threshold1"]),
                    double.Parse(EmguParameters.Item["Canny2Threshold2"]),
                    Int32.Parse(EmguParameters.Item["Canny2ApertureSize"]),
                    bool.Parse(EmguParameters.Item["Canny2I2Gradient"]));
                if (saveFile)
                {
                    _edged.Save(Utils.String.FilePostfix(_imgPath, "-3-edge"));
                }
            }
            else
            {
                _edged = raw;
            }

            Circles2nd = CvInvoke.HoughCircles(_edged, HoughType.Gradient,
                double.Parse(EmguParameters.Item["Hough2Dp"]),
                double.Parse(EmguParameters.Item["Hough2MinDist"]),
                double.Parse(EmguParameters.Item["Hough2Param1"]),
                double.Parse(EmguParameters.Item["Hough2Param2"]),
                Int32.Parse(EmguParameters.Item["Hough2MinRadius"]),
                Int32.Parse(EmguParameters.Item["Hough2MaxRadius"]));
            Circles2nd = Sort(Circles2nd);
            FilteredCircles2nd = new List<CircleF>();
            foreach (var circle in Circles2nd)
            {
                int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
                if (strength > 30)
                {
                    FilteredCircles2nd.Add(circle);
                }
            }

            watch.Stop();
            msgBuilder.Append(string.Format("{0} Hough circles - {1} ms; ", testName, watch.ElapsedMilliseconds));
            _log.Debug(msgBuilder.ToString());
        }
        private void FFTAnalyze(string path)
        {
            Stopwatch watch = Stopwatch.StartNew();
            _imgPath = path;
            _rawImg = EmguIntfs.Load(path);
            _grayedUmat = EmguIntfs.Grayed(_rawImg);
            Image<Gray, Byte> _edged;
            _edged = EmguIntfs.Canny(EmguIntfs.ToImage(_grayedUmat),
                double.Parse(EmguParameters.Item["Canny1Threshold1"]),
                double.Parse(EmguParameters.Item["Canny1Threshold2"]),
                Int32.Parse(EmguParameters.Item["Canny1ApertureSize"]),
                bool.Parse(EmguParameters.Item["Canny1I2Gradient"]));
            _edged.Save(Utils.String.FilePostfix(_imgPath, "-1-edge"));
        }
        public EmguCircleImage(string path, string testType, int[] minmax)
        {
            switch (testType)
            {
                case "NFT":
                    NFTAnalyze(path);
                    break;
                case "FFT":
                    FFTAnalyze(path);
                    break;
            }
        }
        public Bitmap DrawCircles()
        {
            bool showFirstResult = bool.Parse(EmguParameters.Item["ShowFirstResult"]);

            _log.Debug("Start DrawCircles");
            Mat circleImage = _rawImg.Mat;

            if (showFirstResult)
            {
                for (int i = 0; i < FilteredCircles.Count; i++)
                {
                    CvInvoke.Circle(circleImage, Point.Round(FilteredCircles[i].Center), (int)FilteredCircles[i].Radius, new Bgr(Color.Yellow).MCvScalar, 1);
                }
            }

            for (int i = 0; i < FilteredCircles2nd.Count; i++)
            {
                CvInvoke.Circle(circleImage, Point.Round(FilteredCircles2nd[i].Center), (int)FilteredCircles2nd[i].Radius, new Bgr(Color.Red).MCvScalar, 1);
                CvInvoke.PutText(circleImage, i.ToString("D3"), new Point((int)FilteredCircles2nd[i].Center.X, (int)FilteredCircles2nd[i].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
            }
            circleImage.Save(Utils.String.FilePostfix(_imgPath, "-result"));

            return circleImage.Bitmap;
        }
        public CircleF[] Sort(CircleF[] circles)
        {
            List<CircleF> temp = circles.ToList();
            temp.Sort((c1,c2) => {
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
        public void FilterOnStrength(double threshold = 0)
        {
            _log.Debug("Start FilterOnStrength");
            StringBuilder msgBuilder = new StringBuilder("Circles: " + Environment.NewLine);

            int reference = 0;
            FilteredCircles = new List<CircleF>();
            FilteredLights = new List<int>();

            Lights = new int[Circles.Length];
            for (int i = 0; i < Circles.Length; i++)
            {
                Lights[i] = EmguIntfs.CountPixels(EmguIntfs.ToImage(_grayedUmat), Circles[i]);

                if (i == 0)
                {
                    reference = Lights[i];
                }
                if (Lights[i] > reference * threshold)
                {
                    FilteredLights.Add(Lights[i]);
                    FilteredCircles.Add(Circles[i]);
                }

                //msgBuilder.Append(string.Format("{0} {1}", i.ToString("D3"), Lights[i]) + Environment.NewLine);
            }
            _log.Debug(msgBuilder.ToString());
        }
        public Dictionary<string, string> StatisticInfo()
        {
            Dictionary<string, string> info = new Dictionary<string, string>();

            info["MaxRadius"] = FilteredCircles.ToList().Max(circle => circle.Radius).ToString();
            info["MinRadius"] = FilteredCircles.ToList().Min(circle => circle.Radius).ToString();
            info["StdEvRadius"] = Utils.Math.StdEv(FilteredCircles.ToList().Select(x => (double)x.Radius).ToList()).ToString("F3");
            //info["MaxLight"] = FilteredLights.ToList().Max().ToString();
            //info["MinLight"] = FilteredLights.ToList().Min().ToString();
            //info["StdEvLight"] = Utils.Math.StdEv(FilteredLights.ToList().Select(x => (double)x).ToList()).ToString("F3");

            return info;
        }
    }
}
