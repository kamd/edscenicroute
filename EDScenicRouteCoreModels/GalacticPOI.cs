using System;

namespace EDScenicRouteCoreModels
{
    [Serializable]
    public class GalacticPOI : IGalacticPoint
    {

        public int Id { get; set; }
        public GalacticPOIType Type { get; set; }
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
