using System.Collections.Generic;
using Utils;

namespace Hardware
{
    public class Equipment
    {
        protected Logger _log;
        protected string Type;
        protected string Name;
        public Equipment(object param)
        {
            Type = "Equipment";
            Name = param as string;

            AssginLogger();
        }
        protected void AssginLogger()
        {
            _log = new Logger(string.Format("{0}({1})", Type, Name));
        }
        private Command _currentCmd;
        protected string CurrentCmd
        {
            get {
                return string.Format("{0}({1}) {2}", Type, Name, _currentCmd);
            }
        }
        virtual protected void PreExecute(Command cmd)
        {
            _currentCmd = cmd;
            _log.Info(CurrentCmd);
        }
        virtual protected void PostExecute(Result result)
        {
            if (result.Id != "Ok")
            {
                _log.Warn(CurrentCmd + " " + result.ToString());
            }
            else if (!string.IsNullOrEmpty(result.Desc))
            {
                _log.Info(CurrentCmd + " " + result.ToString());
            }
        }
        virtual protected Result _execute(Command cmd)
        {
            return new Result("Ok");
        }
        virtual public Result Execute(Command cmd)
        {
            PreExecute(cmd);
            Result result = _execute(cmd);
            PostExecute(result);

            return result;
        }
        virtual public Result AsyncExecute(Command cmd)
        {
            PreExecute(cmd);
            Result result = new Result("Ok");
            PostExecute(result);

            return result;
        }
    }

    public class Port : Equipment
    {
        public Port(object param) : base(param)
        {
            Type = "Port";
            AssginLogger();
        }
        virtual public Result Send(string bytes)
        {
            return Execute(new Command("Send", new Dictionary<string, string>() {
                { "Bytes", bytes }
            }));
        }
    }

    public class PowerSupply : Equipment
    {
        public Port PPort;
        public PowerSupply(object param) : base(param)
        {
            Type = "PowerSupplys";
            AssginLogger();
        }
        protected override Result _execute(Command cmd)
        {
            Result result;
            if (PPort != null)
            {
                if (cmd.Id == "PowerOn")
                {
                    result = PPort.Send("power on bytes");
                }
                else
                {
                    result = new Result("Fail", "Command do not support");
                }
            }
            else
            {
                result = new Result("Fail", "Port not set");
            }

            return result;
        }
    }
}