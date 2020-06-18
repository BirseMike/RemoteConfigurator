using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Windows;
using ConfigurationClasses;

namespace SampleConfigProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var configurator = GetConfiguration();
            var appSettings = configurator.GetAppSettings();


            text_config_keys.Text = string.Join(",", appSettings.AllKeys);

            text_config_values.Text = string.Join(",", appSettings.AllKeys.Select(k => appSettings[k].Value));

        }

        private static IConfigurator GetConfiguration()
        {
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
           
            if (false)
            {
                //Desktop App
                path = @"C:\Users\mike_\source\repos\RemoteConfigurator\SampleConfigApp\bin\Debug";
            }
            else
            {
                //Website
                path = @"C:\Users\mike_\source\repos\RemoteConfigurator\SampleWebsite\bin\";
            }

            var fileList = Directory.GetFiles(path, "*.*").Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));
            foreach (string dll in fileList)
                allAssemblies.Add(Assembly.LoadFile(dll));


            var type = typeof(IConfigurator);

            var types = allAssemblies
                .SelectMany(s =>
                {
                    try
                    {
                        Console.WriteLine(s.FullName);
                        return s.GetTypes();
                    }
                    catch
                    {
                        return new Type[0];
                    }
                })
                .Where(p => type.IsAssignableFrom(p));

            var assemblies = allAssemblies.Where(s => s.GetTypes().Any(t => types.Contains(t)));
            var typelist = types.ToList();
            var firstType = types.First();

            return Activator.CreateInstance(firstType) as IConfigurator;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var configurator = GetConfiguration();
            var appSettings = configurator.GetAppSettings();

            var key = appSettings.AllKeys.Last();
            configurator.SetAppValue(key, $"Setting Value updated @ {DateTime.Now}");
            Button_Click(sender, e);

        }
    }
}
