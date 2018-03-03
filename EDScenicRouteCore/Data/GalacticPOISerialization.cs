using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EDScenicRouteCore.Data
{
    public class GalacticPOISerialization
    {

        public static void SaveToFile(List<GalacticPOI> pois, string filePath)
        {
            var writer = new XmlSerializer(typeof(List<GalacticPOI>));
            using (FileStream file = File.Create(filePath))
            {
                writer.Serialize(file, pois);
            }
        }

        public static List<GalacticPOI> LoadFromFile(string filePath)
        {
            var writer = new XmlSerializer(typeof(List<GalacticPOI>));
            using (var stream = new StreamReader(filePath))
            {
                var pois = (List<GalacticPOI>)writer.Deserialize(stream);
                return pois;
            }
        }
    }
}
