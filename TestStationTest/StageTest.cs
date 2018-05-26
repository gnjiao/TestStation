using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using TestStation;
using System.Diagnostics;

namespace TestStationTest
{
    [TestClass]
    public class StageTest
    {
        [TestMethod]
        public void TestStage_NewStage()
        {
            Stage s = new StageIdle();
            s.OnEvent(new Event("EquipmentDetected"));
        }
    }
}
