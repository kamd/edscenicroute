using System;
using System.Numerics;

namespace EDScenicRouteCoreModels
{
    [Serializable]
    public class GalacticPOI : IGalacticPoint
    {

        public GalacticPOI() { }

        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string GalMapSearch { get; set; }
        public string GalMapUrl { get; set; }
        public Vector3 Coordinates { get; set; }
        public float DistanceFromSol { get; set; }

        public override string ToString()
        {
            return "GalacticPOI: Id = " + Id;
        }
    }
}
