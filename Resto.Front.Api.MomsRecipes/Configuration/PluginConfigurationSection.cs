using System.Configuration;

namespace Resto.Front.Api.MomsRecipes.Configuration
{
    public class PluginConfigurationSection : ConfigurationSection
    {
        private const string PluginConfigurationElementKey = "service-config"; 

        [ConfigurationProperty(PluginConfigurationElementKey, IsRequired = true)]
        public PluginConfigurationElement Config
        {
            get { return (PluginConfigurationElement)this[PluginConfigurationElementKey]; }
            set { this[PluginConfigurationElementKey] = value; }
        }

        
    }
}
