using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ConfigurationClasses
{
    public static class SimpleXmlSerializer
    {
        public static void SerializeObject<T>(T obj, Stream output)
        {
            output.SetLength(0);
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


    }
}
