using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleConfigApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new SampleConfigurator();
            var settings = config.GetAppSettings();
            var value = settings["MyAppSetting"];
            Console.WriteLine($"My value : {value.Value}");

            config.SetAppValue(value.Key, "Banana - changed");

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            settings = configFile.AppSettings.Settings;
            settings["MyAppSetting"].Value = "Banana - changed CM";
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

        }
    }
}
