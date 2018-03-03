using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EDScenicRouteCore.Data
{
    [Serializable]
    public class GalacticPOI : IGalacticPoint
    {
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
