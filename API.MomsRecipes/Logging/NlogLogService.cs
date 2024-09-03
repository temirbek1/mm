using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Resto.Front.Api.Plugin.Core.Logging.Abstract;

namespace Resto.Front.Api.Core.Logging
{
    public class NlogLogService : ILogService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void Info(string message)
        {
            logger.Info(message);
        }

        public void InfoFormat(string format, object arg)
        {
            
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void InfoFormat(string format, params object[] args)
        {
            
        }

        public void Warn(string message)
        {
            logger.Debug(message);
        }

        public void WarnFormat(string format, object arg)
        {
            
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void WarnFormat(string format, params object[] args)
        {
            
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            logger.Error(exception, message);
        }

        public void ErrorFormat(string format, object arg)
        {
            
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            
        }

        public void ErrorFormat(string format, params object[] args)
        {
            
        }
    }
}
