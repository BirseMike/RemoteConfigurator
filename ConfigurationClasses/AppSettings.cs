using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConfigurationClasses
{
    public class KeyValue
    {
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    [XmlRoot("appSettings")]
    public class AppSettings
    {
        [XmlElement("add")]
        public List<KeyValue> Settings { get; set; }

        public AppSettings()
        {
            Settings = new List<KeyValue>();
        }
    }
}
