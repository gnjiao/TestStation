using System;
using System.Collections.Generic;
using Utils;

namespace Hardware
{
    public abstract class Camera : Equipment
    {
        public Camera(object param) : base(param)
        {
            Type = "Camera";
            AssignLogger();
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "Open":
                    return Open();
                case "Close":
                    return Close();
                case "Read":
                    return Read(cmd.Param as Dictionary<string, string>);
                case "Config":
                    return Config(cmd.Param as Dictionary<string, string>);
                default:
                    return new Result("Command doesn't support");
            }
        }
        protected abstract Result Open();
        protected abstract Result Read(Dictionary<string, string> param);
        protected abstract Result Config(Dictionary<string, string> param);
        protected abstract Result Close();
    }
}
