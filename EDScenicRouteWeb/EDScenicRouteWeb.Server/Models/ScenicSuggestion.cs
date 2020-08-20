using System;

namespace EDScenicRouteWeb.Server.Models
{
    [Serializable]
    public class ScenicSuggestion
    {
        public ScenicSuggestion() { }

        public ScenicSuggestion(EDScenicRouteWeb.Server.Models.GalacticPOI poi, float extraDistance, float percentageAlongRoute)
        {
            POI = poi;
            ExtraDistance = extraDistance;
            PercentageAlongRoute = percentageAlongRoute;
        }

        public EDScenicRouteWeb.Server.Models.GalacticPOI POI { get; set; }
        public float ExtraDistance { get; set; }
        public float PercentageAlongRoute { get; set; }

        public override string ToString()
        {
            return $"{POI.Name}, extra distance: {ExtraDistance} Ly, {PercentageAlongRoute}% along route";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ScenicSuggestion other)) return false;
            return POI == other.POI && ExtraDistance == other.ExtraDistance && PercentageAlongRoute == other.PercentageAlongRoute;
        }

        public override int GetHashCode()
        {
            return unchecked(POI.GetHashCode() * 17 + ExtraDistance.GetHashCode() * 17 + PercentageAlongRoute.GetHashCode());
        }
    }
}
