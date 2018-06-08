using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        public static void Test()
        {
            const string testImage = @"D:\work\TestStation\ImageTool\Samples\Sample2.jpg";

            StringBuilder msgBuilder = new StringBuilder("Performance: ");
            //Load the image from file and resize it for display
            Image<Bgr, Byte> img = new Image<Bgr, byte>(testImage);
            //.Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

            //Convert the image to grayscale and filter out the noise
            UMat uimage = new UMat();
            CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);

            //use image pyr to remove noise
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(uimage, pyrDown);
            CvInvoke.PyrUp(pyrDown, uimage);
            Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

            Image<Gray, byte> edge = new Image<Gray, byte>(uimage.Cols, uimage.Rows);
            CvInvoke.Canny(uimage, edge, 90, 180);
            edge.Save(@"d:\edge.jpg");

            Stopwatch watch = Stopwatch.StartNew();
            double cannyThreshold = 180;
            double circleAccumulatorThreshold = 120;
            //CircleF[] circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);
            CircleF[] circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2, 20, 180, 15, 18, 22);

            watch.Stop();
            msgBuilder.Append(string.Format("Hough circles - {0} ms; ", watch.ElapsedMilliseconds));

            //draw circles
            Mat circleImage = new Mat(testImage);
            //Mat circleImage = new Mat(img.Size, DepthType.Cv8U, 3);
            //circleImage.SetTo(new MCvScalar(0));
            foreach (CircleF circle in circles)
                CvInvoke.Circle(circleImage, Point.Round(circle.Center), (int)circle.Radius, new Bgr(Color.Brown).MCvScalar, 2);
            circleImage.Save(@"d:\circles.jpg");

            (new Logger()).Debug(msgBuilder.ToString());
        }
    }
}
