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
    }
}
