using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EDScenicRouteCore.Data
{
    public class GalacticSystemSerialization
    {
        public static void SaveToFile(List<GalacticSystem> systems, string filePath)
        {
            var writer = new XmlSerializer(typeof(List<GalacticSystem>));
            using (FileStream file = File.Create(filePath))
            {
                writer.Serialize(file, systems);
            }
        }

        public static List<GalacticSystem> LoadFromFile(string filePath)
        {
            var writer = new XmlSerializer(typeof(List<GalacticSystem>));
            using (var stream = new StreamReader(filePath))
            {
                var systems = (List<GalacticSystem>)writer.Deserialize(stream);
                return systems;
            }
        }
    }
}
