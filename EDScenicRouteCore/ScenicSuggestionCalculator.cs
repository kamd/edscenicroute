using EDScenicRouteCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using EDScenicRouteCoreModels;

using Vector3 = EDScenicRouteCoreModels.Vector3;

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

        public ScenicSuggestionResults GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            var originalDistance = DistanceBetweenPoints(from, to);
            var suggestions = POIs.
                Where(p => p.DistanceFromSol > 200f). // Ignore the "bubble" of near-Earth POIs TODO
                Select(p => new ScenicSuggestion(p, ExtraDistanceIncurred(from, to, p, originalDistance))).
                Where(ss => ss.ExtraDistance <= acceptableExtraDistance && ss.ExtraDistance > 0f).
                OrderBy(ss => ss.ExtraDistance).
                ToList();

            return new ScenicSuggestionResults(){StraightLineDistance = originalDistance, Suggestions = suggestions};
        }

        public static float DistanceFromSol(IGalacticPoint point)
        {
            return DistanceFromSol(point.Coordinates);
        }

        public static float DistanceFromSol(Vector3 point)
        {
            return System.Numerics.Vector3.Distance(ToNumericsVector3(point), System.Numerics.Vector3.Zero);
        }

        private static float ExtraDistanceIncurred(GalacticSystem a, GalacticSystem b, GalacticPOI poi, float originalDistance)
        {
            var distanceViaPOI = DistanceBetweenPoints(a, poi) + DistanceBetweenPoints(poi, b);
            return distanceViaPOI - originalDistance;
        }

        private static float DistanceBetweenPoints(IGalacticPoint a, IGalacticPoint b)
        {
            return System.Numerics.Vector3.Distance(ToNumericsVector3(a.Coordinates), ToNumericsVector3(b.Coordinates));
        }

        private static System.Numerics.Vector3 ToNumericsVector3(Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
        }


    }
}
