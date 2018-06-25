using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStation;
using System.Collections.Generic;
using Utils;
using Hardware;

namespace TestStationTest
{
    class TestStageOwner : StageOwner
    {
        public TestStageOwner(object param) : base(param)
        {
            Name = param as string;
            AssignLogger();

            Stages = new Dictionary<string, Stage>() {
                { "Idle", new StageIdle(this) },
                { "Ready", new StageReady(this) },
                { "Loaded", new StageLoaded(this) },
                { "Testing", new StageTesting(this) },
                { "TestPass", new StageTestPass(this) },
                { "TestFail", new StageTestFail(this) },
                { "Pause", new StagePause(this) },
            };
            CurStage = Stages["Idle"];
        }
        public override Result SetStage(string name)
        {
            CurStage = Stages[name];
            return new Result("Ok");
        }
    }
    [TestClass]
    public class StageTransferTest
    {
        [TestMethod]
        public void TestStage_StageIdle_ToReady()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");

            o.OnEvent(new Event("EquipmentOk"));
            Assert.IsTrue(o.CurStage.Name == "Ready");
        }
        [TestMethod]
        public void TestStage_StageReady_ToIdle()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));

            o.OnEvent(new Event("EquipmentFail"));
            Assert.IsTrue(o.CurStage.Name == "Idle");
        }
        [TestMethod]
        public void TestStage_StageReady_ToLoaded()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            Assert.IsTrue(o.CurStage.Name == "Loaded");
        }
        [TestMethod]
        public void TestStage_StageLoaded_ToTesting()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            o.OnEvent(new Event("StartTest"));
            Assert.IsTrue(o.CurStage.Name == "Testing");
        }
        [TestMethod]
        public void TestStage_StageTesting_ToTestPass()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            o.OnEvent(new Event("StartTest"));
            o.OnEvent(new Event("TestPass"));
            Assert.IsTrue(o.CurStage.Name == "TestPass");
        }
        [TestMethod]
        public void TestStage_StageTesting_ToTestFail()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            o.OnEvent(new Event("StartTest"));
            o.OnEvent(new Event("TestFail"));
            Assert.IsTrue(o.CurStage.Name == "TestFail");
        }
        [TestMethod]
        public void TestStage_StageTesting_ToPause()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            o.OnEvent(new Event("StartTest"));
            o.OnEvent(new Event("Pause"));
            Assert.IsTrue(o.CurStage.Name == "Pause");
        }
        [TestMethod]
        public void TestStage_StageTestPass_ToReady()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            o.OnEvent(new Event("StartTest"));
            o.OnEvent(new Event("TestPass"));
            o.OnEvent(new Event("RemoveDut"));
            Assert.IsTrue(o.CurStage.Name == "Ready");
        }
        [TestMethod]
        public void TestStage_StageTestFail_ToLoaded()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            o.OnEvent(new Event("StartTest"));
            o.OnEvent(new Event("TestFail"));
            o.OnEvent(new Event("Recover"));
            Assert.IsTrue(o.CurStage.Name == "Ready");
        }
        [TestMethod]
        public void TestStage_StageTestFail_ToReady()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");
            o.OnEvent(new Event("EquipmentOk"));
            o.OnEvent(new Event("AddDut", null/*DUT*/));
            o.OnEvent(new Event("StartTest"));
            o.OnEvent(new Event("TestFail"));
            o.OnEvent(new Event("RemoveDut"));
            Assert.IsTrue(o.CurStage.Name == "Ready");
        }
    }
    [TestClass]
    public class StageControlEquipmentTest
    {
        [TestMethod]
        public void StageIdle()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Camera c = new M8051("M8051");
        }
    }
}
