using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Hardware
{
    public class HardwareCommand : Command
    {
        public string Type;
        public string Name;
        public HardwareCommand(string type, string name, string id, Dictionary<string, string> param = null) : base(id, param)
        {
            Type = type;
            Name = name;
        }
        public override string ToString()
        {
            return string.Format("{0}({1}) {2}", Type, Name, base.ToString());
        }
    }
    public class HardwareSrv : CommandHandler
    {
        #region singleton
        private HardwareSrv() : base("")
        {
            Type = "HardwareSrv";
            AssignLogger();

            _log.Debug(string.Format("HardwareSrv(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
        }
        private static HardwareSrv _instance;
        public static HardwareSrv GetInstance()
        {
            if (_instance == null)
            {
                _instance = new HardwareSrv();
            }
            return _instance;
        }
        #endregion
        private List<Equipment> _equipments = new List<Equipment>();
        public void Add(Equipment e)
        {
            _equipments.Add(e);
        }
        public Equipment Get(string type, string name = null)
        {
            return _equipments.Find(x => x.Type == type && (name == null || x.Name == name));
        }
        protected override Result _execute(Command cmd)
        {
            HardwareCommand hcmd = cmd as HardwareCommand;
            return Get(hcmd.Type, hcmd.Name).Execute(cmd);
        }
    }
}
