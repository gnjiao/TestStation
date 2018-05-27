using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStation;
using System.Collections.Generic;

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
                { "Testing", new StageTesting(this) },
                { "TestPass", new StageTestPass(this) },
                { "TestFail", new StageTestFail(this) },
            };
            CurStage = Stages["Idle"];
        }
        public override void SetStage(string name)
        {
            CurStage = Stages[name];
        }
    }
    [TestClass]
    public class StageTest
    {
        [TestMethod]
        public void TestStage_StageIdle_ToReady()
        {
            StageOwner o = new TestStageOwner("TestStation");
            Assert.IsTrue(o.CurStage.Name == "Idle");

            o.OnEvent(new Event("EquipmentOk"));
            Assert.IsTrue(o.CurStage.Name == "Ready");
        }
    }
}
