using System;
using System.Collections.Generic;
using System.Text;

namespace EDScenicRouteCore.Data
{
    public class ScenicSuggestion
    {
        public ScenicSuggestion(GalacticPOI poi, float extraDistance)
        {
            POI = poi;
            ExtraDistance = extraDistance;
        }

        public GalacticPOI POI { get; set; }
        public float ExtraDistance { get; set; }

        public override string ToString()
        {
            return $"{POI.Name}, extra distance: {ExtraDistance} Ly";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ScenicSuggestion other)) return false;
            return POI == other.POI && ExtraDistance == other.ExtraDistance;
        }

        public override int GetHashCode()
        {
            return unchecked(POI.GetHashCode() * 17 + ExtraDistance.GetHashCode());
        }
    }
}
