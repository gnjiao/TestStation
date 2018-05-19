using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessor;
using JbImage;
using System.Drawing;

namespace ImageTest
{
    [TestClass]
    public class ImageProcessorTest
    {
        [TestMethod]
        public void LoadBmp()
        {
            string path = @"D:\work\TestStation\ImageTest\test\circle.bmp";
            var imageFactory = new ImageFactory().Load(path);

        }
        [TestMethod]
        public void TestBinarize()
        {
            string path = @"D:\work\TestStation\ImageTest\test\small.bmp";
            Image i = (Image)Preprocess.Binarize(path);
            i.Save(@"D:\work\TestStation\ImageTest\test\smallBin.bmp");
        }
        [TestMethod]
        public void Test()
        {
            string path = @"D:\work\TestStation\ImageTest\test\";

            byte[][] array = Preprocess.ToArray((Bitmap)Bitmap.FromFile(path + "small1.bmp"));
            CirclesFinder c = new CirclesFinder(array);
            c.Draw(path + "small1Result.bmp");

            array = Preprocess.ToArray((Bitmap)Bitmap.FromFile(path + "small3.bmp"));
            c = new CirclesFinder(array);
            c.Draw(path + "small3Result.bmp");

            array = Preprocess.ToArray((Bitmap)Bitmap.FromFile(path + "smallx.bmp"));
            c = new CirclesFinder(array);
            c.Draw(path + "smallxResult.bmp");

            array = Preprocess.ToArray((Bitmap)Bitmap.FromFile(path + "smallxerror1.bmp"));
            c = new CirclesFinder(array);
            c.Draw(path + "smallxerror1Result.bmp");

            array = Preprocess.ToArray((Bitmap)Bitmap.FromFile(path + "smallxerror2.bmp"));
            c = new CirclesFinder(array);
            c.Draw(path + "smallxerror2Result.bmp");
        }
    }
}
