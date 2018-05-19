using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JbImage;

namespace ImageTest
{
    /// <summary>
    /// CirclesFinderTest 的摘要说明
    /// </summary>
    [TestClass]
    public class CirclesFinderTest
    {
        public CirclesFinderTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestLinesFinder_Length1()
        {
            byte[] array = new byte[] { 0 };
            List<Line> lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 0);

            array = new byte[] { 1 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 0);
            Assert.IsTrue(lines[0].End == 0);
            Assert.IsTrue(lines[0].Length == 1);

            array = new byte[] { 0, 1 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 1);
            Assert.IsTrue(lines[0].End == 1);
            Assert.IsTrue(lines[0].Length == 1);

            array = new byte[] { 1, 0 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 0);
            Assert.IsTrue(lines[0].End == 0);
            Assert.IsTrue(lines[0].Length == 1);

            array = new byte[] { 0, 0, 0 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 0);

            array = new byte[] { 0, 0, 1 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 2);
            Assert.IsTrue(lines[0].End == 2);

            array = new byte[] { 0, 1, 0 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 1);
            Assert.IsTrue(lines[0].End == 1);

            array = new byte[] { 1, 0, 0 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 0);
            Assert.IsTrue(lines[0].End == 0);
        }
        [TestMethod]
        public void TestLinesFinder_Length2()
        {
            byte[] array = new byte[] { 1,1 };
            List<Line> lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 0);
            Assert.IsTrue(lines[0].End == 1);
            Assert.IsTrue(lines[0].Length == 2);

            array = new byte[] { 0, 1, 1 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 1);
            Assert.IsTrue(lines[0].End == 2);
            Assert.IsTrue(lines[0].Length == 2);

            array = new byte[] { 1, 1, 0 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 1);
            Assert.IsTrue(lines[0].Start == 0);
            Assert.IsTrue(lines[0].End == 1);
            Assert.IsTrue(lines[0].Length == 2);
        }
        [TestMethod]
        public void TestLinesFinder_2Lines()
        {
            byte[] array = new byte[] { 1, 0, 1 };
            List<Line> lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 2);
            Assert.IsTrue(lines[0].Equals(new Line(0)));
            Assert.IsTrue(lines[1].Equals(new Line(2)));

            array = new byte[] { 1, 1, 0, 1 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 2);
            Assert.IsTrue(lines[0].Equals(new Line(0, 1)));
            Assert.IsTrue(lines[1].Equals(new Line(3)));

            array = new byte[] { 1, 0, 1, 1 };
            lines = CirclesFinder.FindLines(array);
            Assert.IsTrue(lines.Count == 2);
            Assert.IsTrue(lines[0].Equals(new Line(0)));
            Assert.IsTrue(lines[1].Equals(new Line(2, 3)));
        }
        [TestMethod]
        public void TestRoundAddLine()
        {
            //Equal///////////////////////////////////////////////
            Round r = new Round();
            r.Lines.Add(new Line(0));
            Line l = new Line(0);

            bool result = l.AddTo(r);

            Assert.IsTrue(result);
            Assert.IsTrue(r.Lines.Count == 2);
            Assert.IsTrue(r.Bottom.Equals(l));

            r = new Round();
            r.Lines.Add(new Line(0, 1));
            l = new Line(0, 1);

            result = l.AddTo(r);

            Assert.IsTrue(result);
            Assert.IsTrue(r.Lines.Count == 2);
            Assert.IsTrue(r.Bottom.Equals(l));
            //left no cross/////////////////////////////////////////////
            r = new Round();
            r.Lines.Add(new Line(1));
            l = new Line(0);

            result = l.AddTo(r);

            Assert.IsTrue(!result);
            Assert.IsTrue(r.Lines.Count == 1);
            Assert.IsTrue(r.Bottom.Equals(new Line(1)));

            r = new Round();
            r.Lines.Add(new Line(2, 5));
            l = new Line(0, 1);

            result = l.AddTo(r);

            Assert.IsTrue(!result);
            Assert.IsTrue(r.Lines.Count == 1);
            Assert.IsTrue(r.Bottom.Equals(new Line(2, 5)));
            //left with cross////////////////////////////////////////////
            r = new Round();
            r.Lines.Add(new Line(0, 1));
            l = new Line(0);

            result = l.AddTo(r);

            Assert.IsTrue(result);
            Assert.IsTrue(r.Lines.Count == 2);
            Assert.IsTrue(r.Bottom.Equals(l));

            r = new Round();
            r.Lines.Add(new Line(0, 5));
            l = new Line(1, 3);

            result = l.AddTo(r);

            Assert.IsTrue(result);
            Assert.IsTrue(r.Lines.Count == 2);
            Assert.IsTrue(r.Bottom.Equals(l));
        }
        [TestMethod]
        public void TestRoundsFinder()
        {
            byte[][] array = new byte[][]
                {
                    new byte[] { 0, 1, 0 },
                    new byte[] { 1, 0, 1 },
                };

            CirclesFinder f = new CirclesFinder(array);

            Assert.IsTrue(f.Rounds.Count == 3);
            Assert.IsTrue(f.Rounds[0].Lines.Count == 1);
            Assert.IsTrue(f.Rounds[0].Bottom.Equals(new Line(1)));
            Assert.IsTrue(f.Rounds[0].StartY == 0);
            Assert.IsTrue(f.Rounds[0].EndY == 0);
            Assert.IsTrue(f.Rounds[0].MaxLenY == 0);
            Assert.IsTrue(f.Rounds[0].MaxLenLine.Equals(new Line(1)));

            Assert.IsTrue(f.Rounds[1].Lines.Count == 1);
            Assert.IsTrue(f.Rounds[1].Bottom.Equals(new Line(0)));
            Assert.IsTrue(f.Rounds[1].StartY == 1);
            Assert.IsTrue(f.Rounds[1].EndY == 1);
            Assert.IsTrue(f.Rounds[1].MaxLenY == 1);
            Assert.IsTrue(f.Rounds[1].MaxLenLine.Equals(new Line(0)));

            Assert.IsTrue(f.Rounds[2].Lines.Count == 1);
            Assert.IsTrue(f.Rounds[2].Bottom.Equals(new Line(2)));
            Assert.IsTrue(f.Rounds[2].StartY == 1);
            Assert.IsTrue(f.Rounds[2].EndY == 1);
            Assert.IsTrue(f.Rounds[2].MaxLenY == 1);
            Assert.IsTrue(f.Rounds[2].MaxLenLine.Equals(new Line(2)));

            array = new byte[][]
                {
                    new byte[] { 1, 1, 0 },
                    new byte[] { 1, 0, 1 },
                };

            f = new CirclesFinder(array);

            Assert.IsTrue(f.Rounds.Count == 2);
            Assert.IsTrue(f.Rounds[0].Lines.Count == 2);
            Assert.IsTrue(f.Rounds[0].Bottom.Equals(new Line(0)));
            Assert.IsTrue(f.Rounds[0].StartY == 0);
            Assert.IsTrue(f.Rounds[0].EndY == 1);
            Assert.IsTrue(f.Rounds[0].MaxLenY == 0);
            Assert.IsTrue(f.Rounds[0].MaxLenLine.Equals(new Line(0, 1)));

            Assert.IsTrue(f.Rounds[1].Lines.Count == 1);
            Assert.IsTrue(f.Rounds[1].Bottom.Equals(new Line(2)));
            Assert.IsTrue(f.Rounds[1].StartY == 1);
            Assert.IsTrue(f.Rounds[1].EndY == 1);
            Assert.IsTrue(f.Rounds[1].MaxLenY == 1);
            Assert.IsTrue(f.Rounds[1].MaxLenLine.Equals(new Line(2)));

            array = new byte[][]
                {
                    new byte[] { 1, 1, 0, 1, 1, 1, 0, 0 },
                    new byte[] { 0, 0, 0, 1, 1, 1, 0, 0 },
                    new byte[] { 1, 1, 0, 1, 1, 1, 0, 0 },
                    new byte[] { 1, 1, 0, 0, 0, 0, 0, 0 },
                    new byte[] { 0, 0, 0, 1, 1, 1, 0, 0 },
                    new byte[] { 1, 1, 0, 0, 1, 0, 0, 1 },
                };

            f = new CirclesFinder(array);

            Assert.IsTrue(f.Rounds.Count == 6);
            Assert.IsTrue(f.Rounds[0].Lines.Count == 1);
            Assert.IsTrue(f.Rounds[0].Bottom.Equals(new Line(0, 1)));
            Assert.IsTrue(f.Rounds[0].StartY == 0);
            Assert.IsTrue(f.Rounds[0].EndY == 0);
            Assert.IsTrue(f.Rounds[0].MaxLenY == 0);
            Assert.IsTrue(f.Rounds[0].MaxLenLine.Equals(new Line(0, 1)));

            Assert.IsTrue(f.Rounds[1].Lines.Count == 3);
            Assert.IsTrue(f.Rounds[1].Bottom.Equals(new Line(3, 5)));
            Assert.IsTrue(f.Rounds[1].StartY == 0);
            Assert.IsTrue(f.Rounds[1].EndY == 2);
            Assert.IsTrue(f.Rounds[1].MaxLenY == 0);
            Assert.IsTrue(f.Rounds[1].MaxLenLine.Equals(new Line(3, 5)));

            Assert.IsTrue(f.Rounds[2].Lines.Count == 2);
            Assert.IsTrue(f.Rounds[2].Bottom.Equals(new Line(0, 1)));
            Assert.IsTrue(f.Rounds[2].StartY == 2);
            Assert.IsTrue(f.Rounds[2].EndY == 3);
            Assert.IsTrue(f.Rounds[2].MaxLenY == 2);
            Assert.IsTrue(f.Rounds[2].MaxLenLine.Equals(new Line(0, 1)));

            Assert.IsTrue(f.Rounds[3].Lines.Count == 2);
            Assert.IsTrue(f.Rounds[3].Bottom.Equals(new Line(4)));
            Assert.IsTrue(f.Rounds[3].StartY == 4);
            Assert.IsTrue(f.Rounds[3].EndY == 5);
            Assert.IsTrue(f.Rounds[3].MaxLenY == 4);
            Assert.IsTrue(f.Rounds[3].MaxLenLine.Equals(new Line(3, 5)));

            Assert.IsTrue(f.Rounds[4].Lines.Count == 1);
            Assert.IsTrue(f.Rounds[4].Bottom.Equals(new Line(0, 1)));
            Assert.IsTrue(f.Rounds[4].StartY == 5);
            Assert.IsTrue(f.Rounds[4].EndY == 5);
            Assert.IsTrue(f.Rounds[4].MaxLenY == 5);
            Assert.IsTrue(f.Rounds[4].MaxLenLine.Equals(new Line(0, 1)));

            Assert.IsTrue(f.Rounds[5].Lines.Count == 1);
            Assert.IsTrue(f.Rounds[5].Bottom.Equals(new Line(7)));
            Assert.IsTrue(f.Rounds[5].StartY == 5);
            Assert.IsTrue(f.Rounds[5].EndY == 5);
            Assert.IsTrue(f.Rounds[5].MaxLenY == 5);
            Assert.IsTrue(f.Rounds[5].MaxLenLine.Equals(new Line(7)));

        }
    }
}
