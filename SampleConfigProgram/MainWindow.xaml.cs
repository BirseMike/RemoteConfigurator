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
            var configurator = GetConfiguration();
            LoadData();
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
                path = @"C:\Users\mike_\source\repos\RemoteConfigurator\SampleWebsite\bin";
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

        private void LoadData()
        {
            var configurator = GetConfiguration();
            DataContext = new
            {
                Files = new string[] { "WebFile" },
                Settings = configurator.GetAppSettings()
            };
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            var configurator = GetConfiguration();
            var appSettings = configurator.GetAppSettings();

            var key = appSettings.AllKeys.Last();
            configurator.SetAppValue(key, $"Setting Value updated @ {DateTime.Now}");
            LoadData();
        }
    }
}
