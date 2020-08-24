using System;
using System.Numerics;
using Newtonsoft.Json;

namespace EDScenicRouteWeb.Server.Models
{
    [Serializable]
    public class GalacticPOI : IGalacticPoint
    {
        public int Id { get; set; }
        public GalacticPOIType Type { get; set; }
        public string Name { get; set; }
        public string GalMapSearch { get; set; }
        public string GalMapUrl { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        [JsonIgnore]
        public Vector3 Coordinates => new Vector3(X, Y, Z);
        public float DistanceFromSol { get; set; }
        public string Body { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }

        public override string ToString()
        {
            return "GalacticPOI: Id = " + Id;
        }
    }
}
