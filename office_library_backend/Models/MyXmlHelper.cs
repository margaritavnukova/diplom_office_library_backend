using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace office_library_backend.Models
{
    public class MyXmlHelper
    {
        public static string SerializeToXml<T>(T data)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new StringWriter())
            {
                using (var writer = new XmlTextWriter(stringWriter))
                {
                    writer.Formatting = System.Xml.Formatting.Indented; // Форматирование для удобства чтения
                    xmlSerializer.Serialize(writer, data);
                    return stringWriter.ToString();
                }
            }
        }
    }
}