using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resto.Front.Api.Plugin.Core.Logging.Abstract
{
    public interface ILogService
    {
        void Info(string message);
        void InfoFormat(string format, object arg);
        void InfoFormat(string format, object arg0, object arg1);
        void InfoFormat(string format, object arg0, object arg1, object arg2);
        void InfoFormat(string format, params object[] args);
        void Warn(string message);
        void WarnFormat(string format, object arg);
        void WarnFormat(string format, object arg0, object arg1);
        void WarnFormat(string format, object arg0, object arg1, object arg2);
        void WarnFormat(string format, params object[] args);
        void Error(string message);
        void Error(string message, Exception exception);
        void ErrorFormat(string format, object arg);
        void ErrorFormat(string format, object arg0, object arg1);
        void ErrorFormat(string format, object arg0, object arg1, object arg2);
        void ErrorFormat(string format, params object[] args);
    }
}
