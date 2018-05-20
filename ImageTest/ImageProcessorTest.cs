using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessor;
using JbImage;
using System.Drawing;

namespace ImageTest
{
    [TestClass]
    public class ImageProcessorTest
    {
        string path = @"D:\work\TestStation\ImageTest\Samples\";
        [Ignore]/* this test takes too much time*/
        [TestMethod]
        public void ProcessorTest_Binarize()
        {
            Image i = (Image)Preprocess.Binarize(path + "Sample2-24b.bmp");
            i.Save(path + "Sample2-24b-bin.bmp");
        }
        [TestMethod]
        public void ProcessorTest_CirclesFinder()
        {
            CirclesFinder c = new CirclesFinder(path + "Sample2-24b-bin.bmp");
            c.Draw(path + "Sample2-result.bmp");
        }
    }
}
