using EDScenicRouteCore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDScenicRouteCore.DataUpdates
{
    public class JSONToPOIConverter
    {

        public List<GalacticPOI> ConvertJSONToPOIs(string json)
        {
            var pois = new List<GalacticPOI>();

            dynamic results = JsonConvert.DeserializeObject<dynamic>(json);
            foreach (var r in results.Children())
            {
                try
                {
                    var coords = (r.coordinates as JArray).Select(c => (float)c).ToArray();
                    pois.Add(new GalacticPOI() {
                        Id = r.id,
                        Name = r.name,
                        GalMapSearch = r.galMapSearch,
                        GalMapUrl = r.galMapUrl,
                        Coordinates = new System.Numerics.Vector3(coords[0], coords[1], coords[2]),
                        Type = r.type
                    });
                } catch(Exception ex)
                {
                    //Skip invalid entry
                    Console.WriteLine(ex.ToString());
                }
                
            }
            return pois;
        }

    }
}
