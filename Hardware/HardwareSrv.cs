using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Hardware
{
    public class HardwareSrv
    {
        #region singleton
        private HardwareSrv()
        {
            Logger log = new Logger(typeof(HardwareSrv));
            log.Debug(string.Format("HardwareSrv(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
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
            return new Equipment("EquipmentName");
        }
    }
}
