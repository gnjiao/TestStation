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
        public static Image<Gray, Byte> EdgeDetect(Image<Gray, Byte> uimage)
        {
            Image<Gray, Byte> edge = new Image<Gray, Byte>(uimage.Cols, uimage.Rows);
            CvInvoke.Canny(uimage, edge, 90, 180);
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
        public static Image<Gray, Byte> Binarize(Image<Gray, Byte> grayImage)
        {
            var threshImage = grayImage.CopyBlank();
            CvInvoke.Threshold(grayImage, threshImage, 30, 255, ThresholdType.Binary);
            return threshImage;
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
        const string testImage = @"D:\work\TestStation\ImageTool\Samples\Sample3.jpg";
        const string resultPath = @"D:\";
        public static void TestBinarize()
        {
            string testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            StringBuilder msgBuilder = new StringBuilder("Performance: ");

            Stopwatch watch = Stopwatch.StartNew();
            Image<Bgr, Byte> img = Load(testImage);
            UMat uimage = Grayed(img);

            Image<Gray, Byte> image = Binarize(ToImage(uimage));
            CircleF[] circles = CvInvoke.HoughCircles(image, HoughType.Gradient, 2, 40, 180, 13, 18, 22);

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
        }
    }
    public class EmguCircleImage
    {
        private Logger _log = new Logger(typeof(EmguCircleImage));
        private string _imgPath;
        private Image<Bgr, Byte> _rawImg;
        private UMat _grayedUmat;
        public CircleF[] Circles;
        public int[] Lights;

        public EmguCircleImage(string path)
        {
            string testName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            StringBuilder msgBuilder = new StringBuilder("Performance: ");
            Image<Gray, Byte> image;

            Stopwatch watch = Stopwatch.StartNew();
            _imgPath = path;
            _rawImg = EmguIntfs.Load(path);
            _grayedUmat = EmguIntfs.Grayed(_rawImg);

            image = EmguIntfs.Binarize(EmguIntfs.ToImage(_grayedUmat));
            Circles = CvInvoke.HoughCircles(image, HoughType.Gradient, 2, 40, 180, 13, 18, 22);

            Sort();

            watch.Stop();
            msgBuilder.Append(string.Format("{0} Hough circles - {1} ms; ", testName, watch.ElapsedMilliseconds));
            _log.Debug(msgBuilder.ToString());
        }
        public Bitmap Draw()
        {
            Mat circleImage = _rawImg.Mat;
            for (int i = 0; i < Circles.Length; i++)
            {
                CvInvoke.Circle(circleImage, Point.Round(Circles[i].Center), (int)Circles[i].Radius, new Bgr(Color.Red).MCvScalar, 2);
                CvInvoke.PutText(circleImage, i.ToString("D3"), new Point((int)Circles[i].Center.X, (int)Circles[i].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
            }
            circleImage.Save(Utils.String.FilePostfix(_imgPath, "-result"));

            return circleImage.Bitmap;
        }
        public void Sort()
        {
            List<CircleF> temp = Circles.ToList();
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
            Circles = temp.ToArray();
        }
        public void Count()
        {
            StringBuilder msgBuilder = new StringBuilder("Circles: " + Environment.NewLine);
            Lights = new int[Circles.Length];
            for (int i = 0; i < Circles.Length; i++)
            {
                Lights[i] = EmguIntfs.CountPixels(EmguIntfs.ToImage(_grayedUmat), Circles[i]);
                msgBuilder.Append(string.Format("{0} {1}", i.ToString("D3"), Lights[i]) + Environment.NewLine);
            }
            _log.Debug(msgBuilder.ToString());
        }
        public Dictionary<string, string> StatisticInfo()
        {
            Dictionary<string, string> info = new Dictionary<string, string>();

            info["MaxRadius"] = Circles.ToList().Max(circle => circle.Radius).ToString();
            info["MinRadius"] = Circles.ToList().Min(circle => circle.Radius).ToString();
            info["StdEvRadius"] = Utils.Math.StdEv(Circles.ToList().Select(x => (double)x.Radius).ToList()).ToString("F3");
            info["MaxLight"] = Lights.ToList().Max().ToString();
            info["MinLight"] = Lights.ToList().Min().ToString();
            info["StdEvLight"] = Utils.Math.StdEv(Lights.ToList().Select(x => (double)x).ToList()).ToString("F3");

            return info;
        }
    }
}
