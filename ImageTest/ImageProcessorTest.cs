using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessor;

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
    }
}
