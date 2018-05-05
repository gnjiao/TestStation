using System.Collections.Generic;
using Utils;

namespace Hardware
{
    public class Command
    {
        public string Id;
        public Dictionary<string, string> Param;
        public Command(string id, Dictionary<string, string> param = null)
        {
            Id = id;
            Param = param;
        }
        public override string ToString()
        {
            string str = Id;
            if (Param != null)
            {
                str += " : ";
                foreach (var p in Param)
                {
                    str += string.Format("{0}({1});", p.Key, p.Value);
                }
            }
            return str;
        }
    }
    public class Result
    {
        public string Id;
        public string Desc;
        public Result(string id, string desc = null)
        {
            Id = id;
            Desc = desc;
        }
        public override string ToString()
        {
            string str = Id;
            if (Desc != null)
            {
                str += " : " + Desc;
            }
            return str;
        }
    }
    public class Equipment
    {
        protected Logger _log;

        protected string Type;
        protected string Name;
        public Equipment(object param)
        {
            Type = "Equipment";
            Name = param as string;

            _log = new Logger("{ 0 }({ 1})");
            _log.Debug(string.Format("Create {0}({1})", Type, Name));
        }

        virtual public Result Execute(Command cmd)
        {
            _log.Debug(string.Format("{0}({1}) {2}", Type, Name, cmd));
            Result result = new Result("Ok");
            _log.Info(string.Format("{0}({1}) {2} {3}", Type, Name, cmd, result));

            return result;
        }
    }
}