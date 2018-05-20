using System.Collections.Generic;
using Utils;

namespace Hardware
{
    public class Equipment : CommandHandler
    {
        public Equipment(object param) : base(param)
        {
            Type = "Equipment";
            AssginLogger();
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