using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

namespace TestStation
{
    public abstract class TestCase : CommandHandler
    {
        public bool _isSkip;
        public bool _isCrucial;
        public TestCase(string name) : base(name)
        {
            Type = "TestCase";
            AssignLogger();
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "Run":
                    return Run();
                case "Config":
                    return Config(cmd.Param);
            }
            return new Result("Ok");
        }
        protected virtual Result Run()
        {
            return new Result("Ok");
        }
        protected virtual Result Config(object param)
        {
            return new Result("Ok");
        }
    }
    public abstract class TestSuite : TestCase
    {
        protected List<TestCase> _cases = new List<TestCase>();
        public TestSuite(string name) : base(name)
        {
            Type = "TestSuite";
            AssignLogger();
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "Run":
                    List<Result> results = new List<Result>();

                    foreach(var c in _cases)
                    {
                        Result ret = c.Execute(cmd);
                        if (ret.Id != "Ok" && c._isCrucial)
                        {
                            return new Result("Fail", "", results);
                        }
                        else
                        {
                            ret.Desc = c.Name;
                            results.Add(ret);
                        }
                    }

                    if (results.Exists(x => x.Id == "Fail"))
                    {
                        return new Result("PartFail", "", results);
                    }
                    else
                    {
                        return new Result("Ok", "", results);
                    }

                case "Add":
                    _cases.Add(cmd.Param as TestCase);
                    return new Result("Ok");
                case "Remove":
                    _cases.Remove(cmd.Param as TestCase);
                    return new Result("Ok");
                case "Ignore":
                    return new Result("Ok");
            }

            return new Result("Ok");
        }
    }
}