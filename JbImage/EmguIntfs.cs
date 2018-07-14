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
        public static Image<Gray, ushort> ToImage(ushort[] data)
        {
            ushort[,,] raw = new ushort[2472, 3296, 1];
            for (int height = 0; height < 2472; height++)
            {
                for (int width = 0; width < 3296; width++)
                {
                    raw[height, width, 0] = data[height * 3296 + width];
                }
            }

            return new Image<Gray, ushort>(raw);
        }
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
        public static Image<Gray, ushort> PyrRemoveNoise(Image<Gray, ushort> uimage)
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
        public static Image<Gray, ushort> Binarize(int threshold, Image<Gray, ushort> grayImage)
        {
            var threshImage = grayImage.CopyBlank();
            CvInvoke.Threshold(grayImage, threshImage, threshold, 0x3fff, ThresholdType.Binary);
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
        public static void TestFFT()
        {
            string Path = @"D:\work\TestStation\TestStation\Samples\2017.7.11\fft.bmp";
            Image<Bgr, byte> RawImg = EmguIntfs.Load(Path);

            Image<Gray, Byte> _grayedUmat = EmguIntfs.ToImage(EmguIntfs.Grayed(RawImg));

            Image<Gray, Byte> _bin = EmguIntfs.Binarize(30, _grayedUmat);
            _bin.Save(Utils.String.FilePostfix(Path, "-0-bin"));

            //Image<Gray, Byte> _edged = EmguIntfs.Canny(_bin,
            //    30,
            //    60,
            //    3,
            //    false);
            //_edged.Save(Utils.String.FilePostfix(Path, "-1-edge"));

            Image<Gray, byte> tempc = new Image<Gray, byte>(_bin.Width, _bin.Height);
            Image<Gray, byte> d = new Image<Gray, byte>(_bin.Width, _bin.Height);
            VectorOfVectorOfPoint con = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(_bin, con, tempc, RetrType.Tree, ChainApproxMethod.ChainApproxSimple, new Point(0, 0));
            for (int conId = 0; conId < con.Size; conId++)
                CvInvoke.DrawContours(d, con, conId, new MCvScalar(255, 0, 255, 255), 2);
            d.Save(Utils.String.FilePostfix(Path, "-1-contour"));

            List<RotatedRect> rects = new List<RotatedRect>();
            for (int conId = 0; conId < con.Size; conId++)
            {
                if (con[conId].Size > 5)
                {
                    rects.Add(CvInvoke.FitEllipse(con[conId]));
                }
            }

            foreach (var rect in rects)
            {
                CvInvoke.Ellipse(d, rect, new Bgr(Color.White).MCvScalar, 2);
            }
            d.Save(Utils.String.FilePostfix(Path, "-1-rects"));
        }

        public static void Test()
        {
            string Path = @"D:\work\TestStation\TestStation\SingleSample\8.tif";
            Image<Gray, ushort> img = new Image<Gray, ushort>(Path);
            Mat mat = new Mat(Path);

            int max = 0;
            for (int width = 0; width < img.Width; width++)
            {
                for (int height = 0; height < img.Height; height++)
                {
                    max = max > img.Data[height, width, 0] ? max : img.Data[height, width, 0];
                }
            }
            img.Save(Utils.String.FilePostfix(Path.Replace("tif","bmp"), "-raw"));
        }
    }
}
