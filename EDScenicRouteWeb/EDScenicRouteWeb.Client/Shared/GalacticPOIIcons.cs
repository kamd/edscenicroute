using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteWeb.Client.Shared
{
    public static class GalacticPOIIcons
    {
        private static readonly Dictionary<GalacticPOIType, string> icons;

        static GalacticPOIIcons()
        {
            icons = new Dictionary<GalacticPOIType, string>
            {
                {GalacticPOIType.blackHole, "glyphicon glyphicon-exclamation-sign"},
                {GalacticPOIType.deepSpaceOutpost, "glyphicon glyphicon-stats"},
                {GalacticPOIType.geyserPOI, "glyphicon glyphicon-cloud-upload"},
                {GalacticPOIType.historicalLocation, "glyphicon glyphicon-tower"},
                {GalacticPOIType.minorPOI, "glyphicon glyphicon-pushpin"},
                {GalacticPOIType.mysteryPOI, "glyphicon glyphicon-question-sign"},
                {GalacticPOIType.nebula, "glyphicon glyphicon-fire"},
                {GalacticPOIType.organicPOI, "glyphicon glyphicon-grain"},
                {GalacticPOIType.planetFeatures, "glyphicon glyphicon-globe"},
                {GalacticPOIType.planetaryNebula, "glyphicon glyphicon-certificate"},
                {GalacticPOIType.pulsar, "glyphicon glyphicon-cd"},
                {GalacticPOIType.regional, "glyphicon glyphicon-move"},
                {GalacticPOIType.starCluster, "glyphicon glyphicon-option-horizontal"},
                {GalacticPOIType.stellarRemnant, "glyphicon glyphicon-star-empty"},
                {GalacticPOIType.surfacePOI, "glyphicon glyphicon-download-alt"}
            };
        }

        public static string IconClassForGalacticPOIType(GalacticPOIType type) => icons[type];
    }
}
