using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
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
        private DataTable _currentSettings;

        private class SettingView : IEditableObject
        {
            public void BeginEdit()
            {
                throw new NotImplementedException();
            }

            public void CancelEdit()
            {
                throw new NotImplementedException();
            }

            public void EndEdit()
            {
                throw new NotImplementedException();
            }
        }

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
            var settings = _currentConfigurator?.GetAppSettings();

            _currentSettings = new DataTable();
            _currentSettings.Columns.AddRange(
                new[]
                {
                    new DataColumn("Key", typeof(string)),
                    new DataColumn("Value", typeof(string))
                }
            );
            var settingKeys = settings?.AllKeys ?? new string[] { };

            foreach( var keyName in settingKeys)
            {
                _currentSettings.Rows.Add(keyName, settings[keyName].Value);
            }

            DataContext = new
            {
                Files = _configurators.Select(a => a.Value.CodeBase),
                Settings = _currentSettings
            };
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            var settings = _currentConfigurator.GetAppSettings();

            foreach (DataRow row in _currentSettings.Rows)
            {
                var key = row[0].ToString();
                var value = row[1].ToString();
                var originalValue = settings[key].Value;

                if (originalValue!= value)
                {
                    _currentConfigurator.SetAppValue(key, value);
                }

            }
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
