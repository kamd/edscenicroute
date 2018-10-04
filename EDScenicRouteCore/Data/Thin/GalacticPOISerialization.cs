using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.Data
{
    public class GalacticPOISerialization
    {

        public static void SaveToFile(IEnumerable<GalacticPOI> pois, string filePath)
        {
            var writer = new XmlSerializer(typeof(List<GalacticPOI>));
            using (FileStream file = File.Create(filePath))
            {
                writer.Serialize(file, pois.ToList());
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
