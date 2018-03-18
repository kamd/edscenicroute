using EDScenicRouteCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace EDScenicRouteCore
{
    public class ScenicSuggestionCalculator
    {
        public ScenicSuggestionCalculator(List<GalacticPOI> pois, List<GalacticSystem> systems)
        {
            POIs = pois;
            Systems = systems;
        }

        private List<GalacticPOI> POIs { get; set; }

        private List<GalacticSystem> Systems { get; set; }

        public (float, List<ScenicSuggestion>) GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            var originalDistance = DistanceBetweenPoints(from, to);
            var suggestions = POIs.
                Where(p => p.DistanceFromSol > 200f). // Ignore the "bubble" of near-Earth POIs TODO
                Select(p => new ScenicSuggestion(p, ExtraDistanceIncurred(from, to, p, originalDistance))).
                Where(ss => ss.ExtraDistance <= acceptableExtraDistance).ToList();

            return (originalDistance, suggestions);
        }

        public static float DistanceFromSol(IGalacticPoint point)
        {
            return DistanceFromSol(point.Coordinates);
        }

        public static float DistanceFromSol(Vector3 point)
        {
            return Vector3.Distance(point, Vector3.Zero);
        }

        private static float ExtraDistanceIncurred(GalacticSystem a, GalacticSystem b, GalacticPOI poi, float originalDistance)
        {
            var distanceViaPOI = DistanceBetweenPoints(a, poi) + DistanceBetweenPoints(poi, b);
            return distanceViaPOI - originalDistance;
        }

        private static float DistanceBetweenPoints(IGalacticPoint a, IGalacticPoint b)
        {
            return Vector3.Distance(a.Coordinates, b.Coordinates);
        }


    }
}
