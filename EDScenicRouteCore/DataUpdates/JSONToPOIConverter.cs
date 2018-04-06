using EDScenicRouteCore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDScenicRouteCoreModels;

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
                    if(!Enum.TryParse((string)r.type, out GalacticPOIType poiType))
                    {
                        continue;
                    }
                    var coords = (r.coordinates as JArray).Select(c => (float)c).ToArray();
                    var coordsVector = new Vector3(coords[0], coords[1], coords[2]);
                    pois.Add(new GalacticPOI()
                    {
                        Id = r.id,
                        Name = r.name,
                        GalMapSearch = r.galMapSearch,
                        GalMapUrl = r.galMapUrl,
                        Coordinates = coordsVector,
                        Type = poiType,
                        DistanceFromSol = ScenicSuggestionCalculator.DistanceFromSol(coordsVector)
                    });
                }
                catch (Exception ex)
                {
                    //Skip invalid entry
                    Console.WriteLine(ex.ToString());
                    throw; // don't skip while I'm testing TODO
                }

            }
            return pois;
        }

    }
}
