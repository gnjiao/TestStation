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
        protected override Result _execute(Command cmd)
        {
            return CurStage.Execute(cmd);
        }
        public virtual Result OnEvent(Event t)
        {
            return _execute(new StageCommand(t));
        }
        public abstract void SetStage(string stageName);
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
                    Owner.SetStage("Ready");
                    break;
            }

            return new Result("Ok");
        }
    }
    public class StageReady : Stage
    {
        public StageReady(StageOwner owner) : base(new StageParam("Ready", owner))
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
    public class StageLoaded : Stage
    {
        public StageLoaded(StageOwner owner) : base(new StageParam("Loaded", owner))
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
    public class StageTesting : Stage
    {
        public StageTesting(StageOwner owner) : base(new StageParam("Testing", owner))
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
    public class StageTestPass : Stage
    {
        public StageTestPass(StageOwner owner) : base(new StageParam("TestPass", owner))
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
    public class StageTestFail : Stage
    {
        public StageTestFail(StageOwner owner) : base(new StageParam("TestFail", owner))
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
}
