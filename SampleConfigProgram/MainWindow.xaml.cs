using System;
using System.Collections.Generic;
using System.Configuration;
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
        private Dictionary<string, Assembly> _configurators = new Dictionary<string, Assembly>();
        private IConfigurator _currentConfigurator;

        public MainWindow()
        {
            InitializeComponent();
            LoadConfigurators();
            LoadData();
            Projects.SelectedIndex = 0;
        }

        private void LoadConfiguration(string assembly_name)
        {
            _currentConfigurator = null;
            if (assembly_name!=null)
            {
                var assembly = _configurators[assembly_name];

                var firstType = assembly.GetTypes().First(t => typeof(IConfigurator).IsAssignableFrom(t));

                _currentConfigurator = Activator.CreateInstance(firstType) as IConfigurator; 
            }
        }

        private void LoadConfigurators()
        {
            string path = @"C:\Users\mike_\source\repos\RemoteConfigurator\";

            var fileList = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(s => {
                    var fname = Path.GetFileName(s);
                    return s.Contains(@"\bin\") && (fname.StartsWith("Sample")|| fname.StartsWith("Web")) && (fname.EndsWith("dll") || fname.EndsWith("exe"));
                    });

            _configurators.Clear();

            foreach (string dll in fileList)
            {
                try
                {
                    var assembly = Assembly.LoadFile(dll);
                    try
                    {
                        if (assembly.GetTypes().Any(t => typeof(IConfigurator).IsAssignableFrom(t)))
                        {
                            _configurators[assembly.CodeBase] = assembly;
                        }
                    }
                    catch(Exception)
                    {

                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void LoadData()
        {
            DataContext = new
            {
                Files = _configurators.Select(a => a.Value.CodeBase),
                Settings = _currentConfigurator?.GetAppSettings()
            };
        }

        private void ListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            var key = _currentConfigurator.GetAppSettings().AllKeys.Last();
            _currentConfigurator.SetAppValue(key, $"Setting Value updated @ {DateTime.Now}");
            LoadData();
        }

        private void Projects_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var assembly_name = Projects.SelectedValue?.ToString();
            LoadConfiguration(assembly_name);
            LoadData();
        }
    }
}
