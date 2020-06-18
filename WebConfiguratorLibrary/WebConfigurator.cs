using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;
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
            settings.Add("location", AssemblyPath);
            settings.Add("settingsfile", config.AppSettings.File);
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

        private static void SerializeObject<T>(T obj, Stream output)
        {
            var serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
                OmitXmlDeclaration = true
            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

                using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
                {
                    serializer.Serialize(xmlWriter, obj, ns);
                }
        }

        public static T DeserializeObject<T>(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            var obj = (T)serializer.Deserialize(stream);
            return obj;
        }

        public bool SetAppValue(string key, string value)
        {
            var filepath = Directory.GetParent(AssemblyPath) + @"\AppSettings.config";
            var orgAppSettings = File.ReadAllText(filepath);

            
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var appSettings = DeserializeObject<AppSettings>(fs);
                fs.Seek(0, SeekOrigin.Begin);

                var keyValue = appSettings.Settings.FirstOrDefault(k => k.Key == key);
                if (keyValue == null)
                {
                   appSettings.Settings.Add(new KeyValue { Key = key, Value = value });
                }
                else
                {
                    keyValue.Value = value;
                }

                    SerializeObject(appSettings,fs);

            }
            // Get the Web application configuration object.
         //   Configuration config = WebConfigurationManager.OpenMappedWebConfiguration(wcfm, "/");
            //config.AppSettings.File = "AppSettings.config";
            //config.AppSettings.Settings[key].Value = value;
            //config.Save(ConfigurationSaveMode.Modified);

            return true;
        

        }
    }
}