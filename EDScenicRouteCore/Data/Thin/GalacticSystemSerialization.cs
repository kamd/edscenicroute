using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.Data
{
    public class GalacticSystemSerialization
    {
        public static void SaveToFile(IEnumerable<GalacticSystem> systems, string filePath)
        {
            var writer = new XmlSerializer(typeof(List<GalacticSystem>));
            using (FileStream file = File.Create(filePath))
            {
                writer.Serialize(file, systems.ToList());
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
