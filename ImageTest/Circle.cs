using Microsoft.VisualStudio.TestTools.UnitTesting;
using Image;

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
        [TestMethod]
        public void MapArray()
        {
            Circle circle;

            circle = new Circle(new int[][]
            {
                new int[] { 0,0,0 },
                new int[] { 0,1,0 },
                new int[] { 0,0,0 },
            });
            Circle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 1, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 1, 0 });
            Assert.IsTrue(circle.ColStart == 1);
            Assert.IsTrue(circle.ColEnd == 1);
            Assert.IsTrue(circle.RowStart == 1);
            Assert.IsTrue(circle.RowEnd == 1);

            circle = new Circle(new int[][]
            {
                new int[] { 0,0,0 },
                new int[] { 0,1,0 },
            });
            Circle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 1, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 1 });
            Assert.IsTrue(circle.ColStart == 1);
            Assert.IsTrue(circle.ColEnd == 1);
            Assert.IsTrue(circle.RowStart == 1);
            Assert.IsTrue(circle.RowEnd == 1);

            circle = new Circle(new int[][]
            {
                new int[] { 0,0,0,0,0 },
                new int[] { 0,0,0,0,0 },
                new int[] { 0,0,1,0,0 },
                new int[] { 0,0,0,0,0 },
                new int[] { 0,0,0,0,0 },
            });
            Circle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 0, 1, 0, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 0, 1, 0, 0 });
            Assert.IsTrue(circle.ColStart == 2);
            Assert.IsTrue(circle.ColEnd == 2);
            Assert.IsTrue(circle.RowStart == 2);
            Assert.IsTrue(circle.RowEnd == 2);

            circle = new Circle(new int[][]
            {
                new int[] { 0,0,0,0,0 },
                new int[] { 0,1,1,1,0 },
                new int[] { 0,1,1,1,0 },
                new int[] { 0,1,1,1,0 },
                new int[] { 0,0,0,0,0 },
            });
            Circle.Map(circle);
            AssertArrayEqual(circle.ColMap, new int[] { 0, 3, 3, 3, 0 });
            AssertArrayEqual(circle.RowMap, new int[] { 0, 3, 3, 3, 0 });
            Assert.IsTrue(circle.ColStart == 1);
            Assert.IsTrue(circle.ColEnd == 3);
            Assert.IsTrue(circle.RowStart == 1);
            Assert.IsTrue(circle.RowEnd == 3);
        }
    }
}
