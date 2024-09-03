using System.Configuration;

namespace Resto.Front.Api.MomsRecipes.Configuration
{
    public class PluginConfigurationElement : ConfigurationElement
    {
        public const string DebugModeProperty = "DebugMode";

        [ConfigurationProperty(DebugModeProperty, DefaultValue = "false", IsRequired = false)]
        public bool DebugMode
        {
            get { return (bool)this[DebugModeProperty]; }
            set { this[DebugModeProperty] = value; }
        }
    }
}
