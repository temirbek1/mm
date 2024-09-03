using Newtonsoft.Json;
using Resto.Front.Api.Plugin.Core.Logging.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace API
{
    public class Settings
    {
        [JsonObject]
        public class PluginSettings : JSONCommonObject
        {
            [JsonProperty("Login")]
            public String Login { get; set; }

            [JsonProperty("Pass")]
            public String Pass { get; set; }
            [JsonProperty("APIUrl")]
            public String APIUrl { get; set; }

        }

        public PluginSettings pluginSettings;

        private static ILogService _logger;

        public Settings(ILogService logger)
        {
            _logger = logger;
            pluginSettings = LoadMomsRecipesAPI();
        }

        #region MomsRecipesAPI load/save
        private string generateFileName()
        {
            String CurrentDir = Environment.CurrentDirectory;
            if (File.Exists(Path.Combine(CurrentDir, "Plugins", "Resto.Front.Api.MomsRecipes", "Resto.Front.Api.MomsRecipes.dll")))
                return Path.Combine(Path.Combine(CurrentDir, "Plugins", "Resto.Front.Api.MomsRecipes", "MomsRecipes.json"));
            return Path.Combine(CurrentDir, "MomsRecipes.json");
        }
        private PluginSettings LoadMomsRecipesAPI()
        {
            PluginSettings tableProp = new PluginSettings();
            try
            {
                var FileName = generateFileName();
                if (!File.Exists(FileName))
                {
                    tableProp = new PluginSettings() { Login = "tynalieva36@momsrecipes.fun", Pass = "nnYH5LItUWqX", APIUrl = "apiv1.momsrecipes.fun" };
                    // сохраним пустой
                    SaveMomsRecipesAPI(tableProp);
                    return tableProp;
                }
                using (TextReader r = new StreamReader(FileName))
                {
                    tableProp = PluginSettings.deserialize<PluginSettings>(r.ReadToEnd());
                    r.Close();
                }

            }
            catch (Exception ex)
            {
                _logger.Error("GlobalSettingsXML Load() - " + ex.ToString());
                // Save(); 
            };
            return tableProp;
        }

        public Boolean SaveMomsRecipesAPI(PluginSettings p)
        {
            var output = JsonConvert.SerializeObject(p);
            try
            {
                using (TextWriter w = new StreamWriter(generateFileName()))
                {
                    w.Write(output);
                    w.Flush();
                    w.Close();
                }                
            }
            catch (Exception e)
            {
                _logger.Error("SaveMomsRecipesAPI", e);
                return false;
            }
            return true;
        }
        #endregion
    }
}
