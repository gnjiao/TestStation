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
        private List<Equipment> _equipments = new List<Equipment>();
        public HardwareSrv()
        {
            Logger log = new Logger(typeof(HardwareSrv));
            log.Debug(string.Format("HardwareSrv(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
        }
        public void Add(Equipment e)
        {
            _equipments.Add(e);
        }
    }
}
