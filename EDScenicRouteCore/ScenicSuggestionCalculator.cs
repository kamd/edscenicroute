using EDScenicRouteCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using EDScenicRouteCore.Exceptions;
using EDScenicRouteCoreModels;

using Vector3 = EDScenicRouteCoreModels.Vector3;

namespace EDScenicRouteCore
{
    public class ScenicSuggestionCalculator
    {

        public const int MAX_SUGGESTIONS = 500;
        public readonly float bubbleIgnoreRadius;

        public ScenicSuggestionCalculator(IQueryable<GalacticPOI> pois, IQueryable<GalacticSystem> systems, float bubbleIgnoreRadius)
        {
            this.bubbleIgnoreRadius = bubbleIgnoreRadius;
            POIs = pois;
            Systems = systems;
        }

        private IQueryable<GalacticPOI> POIs { get; }

        private IQueryable<GalacticSystem> Systems { get; }

        public ScenicSuggestionResults GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            if (DistanceFromSol(to) < bubbleIgnoreRadius && DistanceFromSol(from) < bubbleIgnoreRadius)
            {
                // Warn user that their trip is very close to Earth and we won't have any useful POI suggestions!
                throw new TripWithinBubbleException();
            }
            var originalDistance = DistanceBetweenPoints(from, to);
            var suggestions = POIs.
                Where(p => p.DistanceFromSol > bubbleIgnoreRadius). // Ignore the "bubble" of near-Earth POIs
                Select(p => new ScenicSuggestion(p, ExtraDistanceIncurred(from, to, p, originalDistance))).
                Where(ss => ss.ExtraDistance <= acceptableExtraDistance && ss.ExtraDistance > 0f).
                OrderBy(ss => ss.ExtraDistance).
                Take(MAX_SUGGESTIONS).
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
