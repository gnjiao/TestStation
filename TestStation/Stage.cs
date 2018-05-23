using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public abstract class Stage : CommandHandler
    {
        public Stage(object param) : base(param)
        {
            Type = "Stage";
            AssginLogger();
        }
        public virtual Result OnEvent(Event e)
        {
            return Execute(new StageCommand(e));
        }
    }
    public class StageIdle : Stage
    {
        public StageIdle() : base("Idle")
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
    public class StageReady : Stage
    {
        public StageReady() : base("Ready")
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
    public class StageLoaded : Stage
    {
        public StageLoaded() : base("Loaded")
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
    public class StageTesting : Stage
    {
        public StageTesting() : base("Testing")
        {
        }
        public override Result Execute(Command cmd)
        {
            return new Result("Ok");
        }
    }
}
