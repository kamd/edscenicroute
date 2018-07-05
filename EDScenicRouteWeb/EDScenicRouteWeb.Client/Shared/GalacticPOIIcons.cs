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
                {GalacticPOIType.blackHole, "fas fa-genderless"},
                {GalacticPOIType.deepSpaceOutpost, "fas fa-building"},
                {GalacticPOIType.geyserPOI, "fas fa-cloud-upload-alt"},
                {GalacticPOIType.historicalLocation, "fas fa-chess-rook"},
                {GalacticPOIType.minorPOI, "fas fa-map-pin"},
                {GalacticPOIType.mysteryPOI, "fas fa-question-circle"},
                {GalacticPOIType.nebula, "fas fa-fire"},
                {GalacticPOIType.organicPOI, "fas fa-tree"},
                {GalacticPOIType.planetFeatures, "fas fa-globe-americas"},
                {GalacticPOIType.planetaryNebula, "fas fa-sun"},
                {GalacticPOIType.pulsar, "fas fa-compact-disc"},
                {GalacticPOIType.regional, "fas fa-arrows-alt"},
                {GalacticPOIType.starCluster, "fas fa-ellipsis-h"},
                {GalacticPOIType.stellarRemnant, "fas fa-compress"},
                {GalacticPOIType.surfacePOI, "fas fa-download"}
            };
        }

        public static string IconClassForGalacticPOIType(GalacticPOIType type) => icons[type];
    }
}
