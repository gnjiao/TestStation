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
            StringBuilder msgBuilder = new StringBuilder("Performance: ");
            Stopwatch watch = Stopwatch.StartNew();

            string _imgPath = @"D:\work\TestStation\ImageTest\Test\FFT.bmp";
            Image<Bgr, byte>_rawImg = EmguIntfs.Load(_imgPath);
            UMat _grayedUmat = EmguIntfs.Grayed(_rawImg);

            int multiple = 1;
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(_grayedUmat, pyrDown);
            multiple *= 2;
            CvInvoke.PyrDown(pyrDown, _grayedUmat);
            multiple *= 2;

            Parameters param = EmguParameters.Params[0];

            Image<Gray, Byte> _bin = EmguIntfs.Binarize(param.BinThreshold,
                EmguIntfs.ToImage(_grayedUmat));

            Image<Gray, Byte> _edged = EmguIntfs.Canny(_bin,
                    param.Canny1Threshold1,
                    param.Canny1Threshold2,
                    param.Canny1ApertureSize,
                    param.Canny1I2Gradient);
            _edged.Save(Utils.String.FilePostfix(_imgPath, "-1-edge"));

            CircleF[] Circles = CvInvoke.HoughCircles(_edged, HoughType.Gradient,
                param.Hough1Dp,
                param.Hough1MinDist,
                param.Hough1Param1,
                param.Hough1Param2,
                param.Hough1MinRadius, param.Hough1MaxRadius);

            #region draw
            Mat _result = _edged.Mat;

            for (int i = 0; i < Circles.Length; i++)
            {
                Point center = Point.Round(Circles[i].Center);
                center.X *= 1;
                center.Y *= 1;
                int radius = (int)Circles[i].Radius * 1;

                //if (2 * radius < _result.Size.Height && 2 * radius < _result.Size.Width)
                {
                    CvInvoke.Circle(_result, center, radius, new Bgr(Color.White).MCvScalar, 1);
                    CvInvoke.PutText(_result, i.ToString("D3"), new Point((int)Circles[i].Center.X, (int)Circles[i].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
                }
            }
            _result.Save(Utils.String.FilePostfix(_imgPath, "-result"));
            #endregion

            watch.Stop();
            msgBuilder.Append(string.Format("{0} Hough circles - {1} ms; ", "FFT", watch.ElapsedMilliseconds));
        }
    }
}
