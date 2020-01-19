using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public static class CacheLib
    {
        private static readonly ILog _log = LogManager.GetLogger("LOGGER");
        public static void Initialize()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        public static void LogError(Exception ex)
        {
            _log.Error(ex.Message);
            _log.Error(ex.StackTrace);
        }
        public static void LogDebug(string message)
        {
            _log.Debug(message);
        }
        public static void LogInfo(string message)
        {
            _log.Info(message);
        }
    }
}
