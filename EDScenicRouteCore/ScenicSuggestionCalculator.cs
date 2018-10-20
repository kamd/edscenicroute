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

        public ScenicSuggestionCalculator(IEnumerable<GalacticPOI> pois, IQueryable<GalacticSystem> systems, float bubbleIgnoreRadius)
        {
            this.bubbleIgnoreRadius = bubbleIgnoreRadius;
            POIs = pois;
            Systems = systems;
        }

        private IEnumerable<GalacticPOI> POIs { get; }

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
                Select(p =>
                {
                    var result = ExtraDistanceIncurred(@from, to, p, originalDistance);
                    return new ScenicSuggestion(p, result.extraDistance, result.percentageAlongRoute);
                }).
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

        private static (float extraDistance, float percentageAlongRoute) ExtraDistanceIncurred(
            GalacticSystem systemA, GalacticSystem systemB, GalacticPOI poi, float originalDistance)
        {
            var a = ToNumericsVector3(systemA.Coordinates);
            var b = ToNumericsVector3(systemB.Coordinates);
            var p = ToNumericsVector3(poi.Coordinates);

            var aToPOI = System.Numerics.Vector3.Distance(a, p);

            if (a == b)
            {
                return (aToPOI, 0f);
            }

            // Find the closest point to the POI along the straight line route, expressed as 0 (closest to a) to 1 (closest to b).
            float distanceSquared = originalDistance * originalDistance;
            float distanceAlongRoute = Math.Max(0f, Math.Min(1f, System.Numerics.Vector3.Dot(p - a, b - a) / distanceSquared));

            var bToPOI = System.Numerics.Vector3.Distance(b, p);
            var distanceViaPOI = aToPOI + bToPOI;
            var extraDistance = distanceViaPOI - originalDistance;
            var approxPercentageAlongRoute = distanceAlongRoute * 100f;
            return (extraDistance, approxPercentageAlongRoute);
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
