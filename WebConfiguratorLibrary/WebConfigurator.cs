using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web.Configuration;
using ConfigurationClasses;

namespace WebConfigurationLibrary
{
    public class WebConfigurator : IConfigurator
    {
        public static string AssemblyPath
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                return Uri.UnescapeDataString(uri.Path);
            }
        }


        public KeyValueConfigurationCollection GetAppSettings()
        {
            WebConfigurationFileMap wcfm = GetConfigurationMapFile();
            
            // Get the Web application configuration object.
            Configuration config = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/","SampleWebsite");

            KeyValueConfigurationCollection settings = config.AppSettings.Settings;
            return settings;
        }

        private static WebConfigurationFileMap GetConfigurationMapFile()
        {
            var configPath = Directory.GetParent(Path.GetDirectoryName(AssemblyPath)).FullName;
            VirtualDirectoryMapping vdm = new VirtualDirectoryMapping(configPath, true);
            WebConfigurationFileMap wcfm = new WebConfigurationFileMap();
            wcfm.VirtualDirectories.Add("/", vdm);
            return wcfm;
        }

        public bool SetAppValue(string key, string value)
        {
            var filepath = Directory.GetParent(Path.GetDirectoryName(AssemblyPath)) + @"\AppSettings.config";
            Console.WriteLine(filepath);
            AppSettingWriter.SaveSettings(filepath, new KeyValue { Key = key, Value = value });
            return true;
        }
    }
}