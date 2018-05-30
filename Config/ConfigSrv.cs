using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Config
{
    public class ConfigSrv
    {
        #region singleton
        protected ConfigSrv()
        {
            Logger log = new Logger(typeof(ConfigSrv));
            log.Debug(string.Format("ConfigSrv(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
        }
        private static ConfigSrv _instance;
        public static ConfigSrv GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConfigSrv();
            }
            return _instance;
        }
        #endregion
        public string Load(string item)
        {
            return "";
        }
        public void Save(string item, string value)
        {   
        }
    }
}