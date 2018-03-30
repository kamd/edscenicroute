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

        private readonly List<string> acceptedTypes = new List<string> {
            "planetaryNebula",
            "nebula",
            "blackHole",
            "historicalLocation",
            "stellarRemnant",
            "planetFeatures",
            "minorPOI",
            "regional",
            "pulsar",
            "starCluster",
            "surfacePOI",
            "deepSpaceOutpost",
            "mysteryPOI",
            "organicPOI",
            "geyserPOI"
        };

        public List<GalacticPOI> ConvertJSONToPOIs(string json)
        {
            var pois = new List<GalacticPOI>();

            dynamic results = JsonConvert.DeserializeObject<dynamic>(json);
            foreach (var r in results.Children())
            {
                try
                {
                    Console.WriteLine(r.id);
                    var type = r.type;
                    if (!acceptedTypes.Contains(type.ToString()))
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
                        Type = r.type,
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
