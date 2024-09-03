using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resto.Front.Api.Plugin.Core.Logging.Abstract;

namespace Resto.Front.Api.MomsRecipes.Core.Logging
{
    public class PluginContextLogService :ILogService
    {        
        public PluginContextLogService()
        {
            
        }

        public void Info(string message)
        {
            PluginContext.Log.Info(message);
        }

        public void InfoFormat(string format, object arg)
        {
            PluginContext.Log.InfoFormat(format, arg);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            PluginContext.Log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            PluginContext.Log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(string format, params object[] args)
        {
            PluginContext.Log.InfoFormat(format, args);
        }

        public void Warn(string message)
        {
            PluginContext.Log.Warn(message);
        }

        public void WarnFormat(string format, object arg)
        {
            PluginContext.Log.WarnFormat(format, arg);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            PluginContext.Log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            PluginContext.Log.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(string format, params object[] args)
        {
            PluginContext.Log.WarnFormat(format, args);
        }

        public void Error(string message)
        {
            PluginContext.Log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            PluginContext.Log.Error(message, exception);
        }

        public void ErrorFormat(string format, object arg)
        {
            PluginContext.Log.ErrorFormat(format, arg);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            PluginContext.Log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            PluginContext.Log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            PluginContext.Log.ErrorFormat(format, args);
        }
    }
}
