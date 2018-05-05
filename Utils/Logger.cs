using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Logger.config", ConfigFileExtension = "config", Watch = true)]
namespace Utils
{
    public class Logger
    {
        public static void Info(string info)
        {
            ILog log = LogManager.GetLogger("TestLogger");//获取一个日志记录器
            log.Warn(DateTime.Now.ToString() + ": login success");//写入一条新log
        }
    }
}
