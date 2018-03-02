using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EDScenicRouteCore.Data
{
    public class GalacticPOI
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string GalMapSearch { get; set; }
        public string GalMapUrl { get; set; }
        public Vector3 Coordinates { get; set; }

        public override string ToString()
        {
            return "GalacticPOI: Id = " + Id;
        }
    }
}
