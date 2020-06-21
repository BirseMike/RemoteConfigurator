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
            return config.AppSettings.Settings;
        }


        public bool SetAppValue(string key, string value)
        {
            var directory = Path.GetDirectoryName(AssemblyPath);
            var filepath = directory + @"\AppSettings.config";
            AppSettingWriter.SaveSettings(filepath, new KeyValue { Key = key, Value = value });
            return true;
        }
    }
}
