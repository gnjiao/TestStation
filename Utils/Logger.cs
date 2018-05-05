using System;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Logger.config", ConfigFileExtension = "config", Watch = true)]
namespace Utils
{
    public class Logger
    {
        protected ILog _log;
        public Logger(Type type = null)
        {
            _log = (type == null) ? LogManager.GetLogger("DefaultLogger") : LogManager.GetLogger(type);
        }
        public Logger(string type)
        {
            _log = LogManager.GetLogger(type);
        }

        public virtual void Debug(string msg)
        {
            _log.Debug(msg);
        }
        public virtual void Info(string msg)
        {
            _log.Info(msg);
        }
        public virtual void Warn(string msg)
        {
            _log.Warn(msg);
        }
        public virtual void Error(string msg, Exception ex)
        {
            _log.Error(msg, ex);
        }
    }
}
