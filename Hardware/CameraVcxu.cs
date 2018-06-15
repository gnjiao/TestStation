using System.Collections.Generic;
using Utils;

namespace Hardware
{
    class Vcxu : Camera
    {
        public Vcxu(object param) : base(param)
        {
            Type = "Vcxu";
            AssignLogger();
        }
        protected override Result Open()
        {
            return new Result("Ok");
        }
        protected override Result Read(Dictionary<string, string> param)
        {
            return new Result("Ok");
        }
        protected override Result Config(Dictionary<string, string> param)
        {
            return new Result("Ok");
        }
        protected override Result Close()
        {
            return new Result("Ok");
        }
    }
}