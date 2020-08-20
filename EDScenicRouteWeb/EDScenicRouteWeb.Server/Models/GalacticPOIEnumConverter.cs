using System.Collections.Generic;

namespace EDScenicRouteWeb.Server.Models
{
    public static class GalacticPOIEnumConverter
    {
        private static readonly Dictionary<GalacticPOIType, string> displayNames;

        static GalacticPOIEnumConverter()
        {
            displayNames = new Dictionary<GalacticPOIType, string>
            {
                {GalacticPOIType.blackHole, "Black Hole"},
                {GalacticPOIType.deepSpaceOutpost, "Deep Space Outpost"},
                {GalacticPOIType.geyserPOI, "Geyser POI"},
                {GalacticPOIType.historicalLocation, "Historical Location"},
                {GalacticPOIType.minorPOI, "Minor POI"},
                {GalacticPOIType.mysteryPOI, "Mystery POI"},
                {GalacticPOIType.nebula, "Nebula"},
                {GalacticPOIType.organicPOI, "Organic POI"},
                {GalacticPOIType.planetFeatures, "Planet Features"},
                {GalacticPOIType.planetaryNebula, "Planetary Nebula"},
                {GalacticPOIType.pulsar, "Pulsar"},
                {GalacticPOIType.regional, "Regional"},
                {GalacticPOIType.starCluster, "Star Cluster"},
                {GalacticPOIType.stellarRemnant, "Stellar Remnant"},
                {GalacticPOIType.surfacePOI, "Surface POI"}
            };
        }

        public static string ConvertToString(GalacticPOIType value)
        {
            return displayNames[value];
        }
    }
}
