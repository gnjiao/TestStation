﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessor;
using JbImage;
using System.Drawing;

namespace ImageTest
{
    [Ignore]/* this test takes too much time*/
    [TestClass]
    public class ImageProcessorTest
    {
        string path = @"D:\work\TestStation\ImageTest\Samples\";
        [TestMethod]
        public void ProcessorTest_Binarize()
        {
            Image i = (Image)ImgProcess.Binarize(path + "Sample2-24b.bmp");
            i.Save(path + "Sample2-24b-bin.bmp");
        }
        [TestMethod]
        public void ProcessorTest_CirclesFinder()
        {
            CirclesFinder c = new CirclesFinder((Bitmap)Bitmap.FromFile(path + "Sample2-24b-bin.bmp"));
            c.Draw(path + "Sample2-result.bmp");
        }
        [TestMethod]
        public void Test_Weist()
        {
            double[] x = new double[] { -3, -2, -1, 0, 1, 2, 3 };
            double[] y = new double[x.Length];

            double w = 1;
            double z = 1;
            for (int i = 0; i < x.Length; i++)
            {
                y[i] = w * System.Math.Sqrt(1 + System.Math.Pow(x[i] / z, 2));
            }
            double result = Utils.Matlab.CalcWeist(x, y);
            Assert.IsTrue(result == w);

            w = 0.5;
            z = 1;
            for (int i = 0; i < x.Length; i++)
            {
                y[i] = w * System.Math.Sqrt(1 + System.Math.Pow(x[i] / z, 2));
            }
            result = Utils.Matlab.CalcWeist(x, y);
            Assert.IsTrue(result == w);

            w = 0.5;
            z = 0.5;
            for (int i = 0; i < x.Length; i++)
            {
                y[i] = w * System.Math.Sqrt(1 + System.Math.Pow(x[i] / z, 2));
            }
            result = Utils.Matlab.CalcWeist(x, y);
            Assert.IsTrue(result == w);
        }
    }
}
