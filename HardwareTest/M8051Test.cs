using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hardware;

namespace HardwareTest
{
    [TestClass]
    public class M8051Test
    {
        [TestMethod]
        public void Open()
        {
            M8051 camera = new M8051();
            camera.Open();
        }
    }
}
