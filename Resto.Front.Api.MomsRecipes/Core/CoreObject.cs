using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using Resto.Front.Api.MomsRecipes.Configuration;

namespace Resto.Front.Api.MomsRecipes.Core
{
    public class CoreObject
    {
        private static readonly object _syncObject = new object();
        private static volatile CoreObject _instance;

        private CoreObject()
        {
            Configuration = ConfigurationManager.GetSection("pluginConfiguration") as PluginConfigurationSection;
        }

        public static CoreObject Instance
        {
            get
            {
                // http://msdn.microsoft.com/en-us/library/ff650316.aspx ( Multithreaded Singleton section )
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new CoreObject();
                        }
                    }
                }

                return _instance;
            }
        }

        public PluginConfigurationSection Configuration { get; }
    }
}
