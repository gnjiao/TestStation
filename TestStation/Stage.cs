using System.Collections.Generic;
using Utils;

namespace TestStation
{
    public class Event
    {
        public string Id;
        public object Param;
        public Event(string id, object param = null)
        {
            Id = id;
            Param = param;
        }
    }
    public class StageCommand : Command
    {
        public StageCommand(Event e) : base(e.Id)
        {
        }
    }
    public abstract class StageOwner : CommandHandler
    {
        protected Dictionary<string, Stage> Stages = new Dictionary<string, Stage>();
        public Stage CurStage;
        public StageOwner(object param) : base(param)
        {
            Type = "Stage";
            AssignLogger();
        }
        public virtual Result OnEvent(Event t)
        {
            return CurStage.OnEvent(t);
        }
        public abstract Result SetStage(string stageName);
    }
    public class StageParam
    {
        public string Name;
        public StageOwner Owner;
        public StageParam(string name, StageOwner owner)
        {
            Name = name;
            Owner = owner;
        }
    }
    public abstract class Stage : CommandHandler
    {
        protected StageOwner Owner;
        public Stage(StageParam param) : base(param.Name)
        {
            Type = "Stage";
            Owner = param.Owner;
            AssignLogger();
        }
        public virtual Result OnEvent(Event e)
        {
            return Execute(new StageCommand(e));
        }
    }
    public class StageIdle : Stage
    {
        public StageIdle(StageOwner owner) : base(new StageParam("Idle", owner))
        {
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "EquipmentOk":
                    return Owner.SetStage("Ready");
            }

            return new Result("CommandError", "Command do not support", cmd);
        }
    }
    public class StageReady : Stage
    {
        public StageReady(StageOwner owner) : base(new StageParam("Ready", owner))
        {
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "EquipmentFail":
                    return Owner.SetStage("Idle");
                case "AddDut":
                    return Owner.SetStage("Loaded");
            }

            return new Result("CommandError", "Command do not support", cmd);
        }
    }
    public class StageLoaded : Stage
    {
        public StageLoaded(StageOwner owner) : base(new StageParam("Loaded", owner))
        {
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "StartTest":
                    return Owner.SetStage("Testing");
            }

            return new Result("CommandError", "Command do not support", cmd);
        }
    }
    public class StageTesting : Stage
    {
        public StageTesting(StageOwner owner) : base(new StageParam("Testing", owner))
        {
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "TestPass":
                    return Owner.SetStage("TestPass");
                case "TestFail":
                    return Owner.SetStage("TestFail");
            }

            return new Result("CommandError", "Command do not support", cmd);
        }
    }
    public class StageTestPass : Stage
    {
        public StageTestPass(StageOwner owner) : base(new StageParam("TestPass", owner))
        {
        }
        protected override Result _execute(Command cmd)
        {
            return new Result("CommandError", "Command do not support", cmd);
        }
    }
    public class StageTestFail : Stage
    {
        public StageTestFail(StageOwner owner) : base(new StageParam("TestFail", owner))
        {
        }
        protected override Result _execute(Command cmd)
        {
            return new Result("CommandError", "Command do not support", cmd);
        }
    }
}
