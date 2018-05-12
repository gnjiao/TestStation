using System.Collections.Generic;

namespace Utils
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
                str = str.Substring(0, str.Length - 1);
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
}