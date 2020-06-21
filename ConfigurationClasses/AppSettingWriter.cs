using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationClasses
{
    public static class AppSettingWriter
    {
        public static void SaveSettings(string filepath, params KeyValue[] keyValues)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var appSettings = SimpleXmlSerializer.DeserializeObject<AppSettings>(fs);
                fs.Seek(0, SeekOrigin.Begin);

                foreach (var kv in keyValues)
                {
                    var keyValue = appSettings.Settings.FirstOrDefault(k => k.Key == kv.Key);
                    if (keyValue == null)
                    {
                        appSettings.Settings.Add(new KeyValue { Key = kv.Key, Value = kv.Value });
                    }
                    else
                    {
                        keyValue.Value = kv.Value;
                    }
                }

                SimpleXmlSerializer.SerializeObject(appSettings, fs);
            }

        }
    }
}
