using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessor;
using JbImage;
using System.Drawing;

namespace ImageTest
{
    [TestClass]
    public class ImageProcessorTest
    {
        string path = @"D:\work\TestStation\ImageTest\test\";
        [Ignore]/* this test takes too much time*/
        [TestMethod]
        public void ProcessorTest_Binarize()
        {
            string path = @"D:\work\TestStation\ImageTest\test\rawTrue.bmp";
            Image i = (Image)Preprocess.Binarize(path);
            i.Save(@"D:\work\TestStation\ImageTest\test\raw.bmp");
        }
        [TestMethod]
        public void ProcessorTest_CirclesFinder()
        {
            CirclesFinder c = new CirclesFinder(path + "small1.bmp");
            c.Draw(path + "small1Result.bmp");

            c = new CirclesFinder(path + "small3.bmp");
            c.Draw(path + "small3Result.bmp");

            c = new CirclesFinder(path + "smallx.bmp");
            c.Draw(path + "smallxResult.bmp");

            c = new CirclesFinder(path + "smallxerror1.bmp");
            c.Draw(path + "smallxerror1Result.bmp");

            c = new CirclesFinder(path + "smallxerror2.bmp");
            c.Draw(path + "smallxerror2Result.bmp");

            c = new CirclesFinder(path + "raw.bmp");
            c.Draw(path + "rawResult.bmp");
        }
        [TestMethod]
        public void ProcessorTest_SpecialCase()
        {
            CirclesFinder c = new CirclesFinder(path + "raw1.bmp");
            c.Draw(path + "raw1Result.bmp");
            c = new CirclesFinder(path + "raw11.bmp");
            c.Draw(path + "raw11Result.bmp");
        }
    }
}
