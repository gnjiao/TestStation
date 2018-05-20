using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbImage;
using Utils;

namespace ImageTest
{
    [TestClass]
    public class ImageTest
    {
        private void AssertArrayEqual(int[] x, int[] y)
        {
            Assert.IsTrue(x.Length == y.Length);

            for (int i = 0; i < x.Length; i++)
            {
                Assert.IsTrue(x[i] == y[i]);
            }
        }
        [Ignore]/* this test takes too much time*/
        [TestMethod]
        public void SingleCircleTest()
        {
            SingleCircle circle;

            circle = new SingleCircle(new int[][]
            {
                new int[] { 0,0,0 },
                new int[] { 0,1,0 },
                new int[] { 0,0,0 },
            });
            SingleCircle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 1, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 1, 0 });
            Assert.IsTrue(circle.ColStart == 1);
            Assert.IsTrue(circle.ColEnd == 1);
            Assert.IsTrue(circle.RowStart == 1);
            Assert.IsTrue(circle.RowEnd == 1);

            SingleCircle.Calc(circle);
            Assert.IsTrue(circle.Radius == 0);
            AssertArrayEqual(circle.Center, new int[] { 1,1 });

            circle = new SingleCircle(new int[][]
            {
                new int[] { 0,0,0 },
                new int[] { 0,1,0 },
            });
            SingleCircle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 1, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 1 });
            Assert.IsTrue(circle.ColStart == 1);
            Assert.IsTrue(circle.ColEnd == 1);
            Assert.IsTrue(circle.RowStart == 1);
            Assert.IsTrue(circle.RowEnd == 1);

            SingleCircle.Calc(circle);
            Assert.IsTrue(circle.Radius == 0);
            AssertArrayEqual(circle.Center, new int[] { 1, 1 });

            circle = new SingleCircle(new int[][]
            {
                new int[] { 0,0,0,0,0 },
                new int[] { 0,0,0,0,0 },
                new int[] { 0,0,1,0,0 },
                new int[] { 0,0,0,0,0 },
                new int[] { 0,0,0,0,0 },
            });
            SingleCircle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 0, 1, 0, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 0, 1, 0, 0 });
            Assert.IsTrue(circle.ColStart == 2);
            Assert.IsTrue(circle.ColEnd == 2);
            Assert.IsTrue(circle.RowStart == 2);
            Assert.IsTrue(circle.RowEnd == 2);

            SingleCircle.Calc(circle);
            Assert.IsTrue(circle.Radius == 0);
            AssertArrayEqual(circle.Center, new int[] { 2, 2 });

            circle = new SingleCircle(new int[][]
            {
                new int[] { 0,0,0,0,0 },
                new int[] { 0,1,1,1,0 },
                new int[] { 0,1,1,1,0 },
                new int[] { 0,1,1,1,0 },
                new int[] { 0,0,0,0,0 },
            });
            SingleCircle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 3, 3, 3, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 3, 3, 3, 0 });
            Assert.IsTrue(circle.ColStart == 1);
            Assert.IsTrue(circle.ColEnd == 3);
            Assert.IsTrue(circle.RowStart == 1);
            Assert.IsTrue(circle.RowEnd == 3);

            SingleCircle.Calc(circle);
            Assert.IsTrue(circle.Radius == 1);
            AssertArrayEqual(circle.Center, new int[] { 2, 2 });

            Logger log = new Logger("UnitTest");
            int size = 10000;
            int min = size / 2 - size / 10;
            int max = size / 2 + size / 10;

            int[][] array = new int[size][];
            for (int i = 0; i<size; i++)
            {
                array[i] = new int[size];
                for (int j = 0; j < size; j++)
                {
                    if ((min < i && i < max) && (min < j && j < max))
                    {
                        array[i][j] = 1;
                    }
                    else
                    {
                        array[i][j] = 0;
                    }
                }
            }
            log.Info("Big Array(" + size + ") Start");
            circle = new SingleCircle(array);

            SingleCircle.Map(circle);
            SingleCircle.Calc(circle);
            log.Info("SingleCircle center : " + circle.Center[0] + "," + circle.Center[1]);
            log.Info("SingleCircle radius: " + circle.Radius);

            log.Info("Big Array End");
        }
    }
}
