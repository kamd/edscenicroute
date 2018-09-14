using EDScenicRouteCore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.DataUpdates
{
    public class JSONToGalacticPointConverter
    {

        public IEnumerable<GalacticPOI> ConvertJSONToPOIs(string filename)
        {
            return ConvertJSONToPoint(filename, POIFromJSON);
        }

        public IEnumerable<GalacticSystem> ConvertJSONToSystems(string filename)
        {
            return ConvertJSONToPoint(filename, SystemFromJSON);
        }

        private IEnumerable<T> ConvertJSONToPoint<T>(string filename, Func<dynamic, T> jsonFunc)
            where T : IGalacticPoint
        {
            using (var jsonStream = new StreamReader(filename))
            {
                using (JsonReader reader = new JsonTextReader(jsonStream))
                {
                    //reader.SupportMultipleContent = true;
                    JsonSerializer serializer = new JsonSerializer();
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            dynamic r = serializer.Deserialize<dynamic>(reader);
                            T poi;
                            try
                            {
                                poi = jsonFunc(r);
                            }
                            catch (Exception ex)
                            {
                                //Skip invalid entry
                                Console.WriteLine(ex.ToString());
                                throw; // don't skip while I'm testing TODO
                            }

                            if (poi != null)
                            {
                                yield return poi;
                            }
                        }
                    }
                }
            }
        }

        private GalacticPOI POIFromJSON(dynamic r)
        {
            if (!Enum.TryParse((string) r.type, out GalacticPOIType poiType))
            {
                return null;
            }

            var coords = (r.coordinates as JArray).Select(c => (float) c).ToArray();
            var coordsVector = new Vector3(coords[0], coords[1], coords[2]);
            return new GalacticPOI()
            {
                Id = r.id,
                Name = r.name,
                GalMapSearch = r.galMapSearch,
                GalMapUrl = r.galMapUrl,
                Coordinates = coordsVector,
                Type = poiType,
                DistanceFromSol = ScenicSuggestionCalculator.DistanceFromSol(coordsVector)
            };
        }

        private GalacticSystem SystemFromJSON(dynamic r)
        {
            //var coords = (r.coords as JArray).Select(c => (float) c).ToArray();
            if (r.coords == null)
            {
                return null;
            }
            var coordsVector = new Vector3((float)r.coords.x, (float)r.coords.y, (float)r.coords.z);
            try
            {
                return new GalacticSystem()
                {
                    Id = r.id,
                    Name = r.name,
                    Coordinates = coordsVector
                };
            }
            catch (Exception exxx)
            {
                Console.WriteLine(exxx);
                throw;
            }
        }
    }
}



