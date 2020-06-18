using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using ConfigurationClasses;

namespace SampleConfigApp
{
    class SampleConfigurator : IConfigurator
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
            var config = ConfigurationManager.OpenExeConfiguration(AssemblyPath);
            config.AppSettings.SectionInformation.AllowOverride = true;
            config.AppSettings.SectionInformation.ConfigSource = "AppSettings.config";

            //ExeConfigurationFileMap configFileMap =
            //    new ExeConfigurationFileMap();
            //configFileMap.ExeConfigFilename = AssemblyPath + ".config";

            //// Get the mapped configuration file
            //var config =
            //   ConfigurationManager.OpenMappedExeConfiguration(
            //     configFileMap, ConfigurationUserLevel.None);

            return config.AppSettings.Settings;
        }


        public bool SetAppValue(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(AssemblyPath);
            config.AppSettings.SectionInformation.ConfigSource = "AppSettings.config";
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            return true;
        }
    }
}
