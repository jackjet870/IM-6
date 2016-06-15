using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IM.Common
{
    public class Logger
    {
        static ILog log;

        static Logger()
        {
            log = log4net.LogManager.GetLogger("Log4netLog");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            if (File.Exists(path))
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            }
            else
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "log4net.config");
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            }
        }

        public static void LogInfo(object msg)
        {
            log.Info(msg);
        }

        public static void LogDebug(object msg)
        {
            log.Debug(msg);
        }

        public static void LogError(object obj, Exception exc)
        {
            log.Error(obj, exc);
        }

        public static void LogError(object obj)
        {
            log.Error(obj);
        }

        public static void ShutDown()
        {
            log4net.LogManager.Shutdown();
        }
    }
}
