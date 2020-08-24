using System;
using System.Collections.Generic;
using System.Linq;
using EDScenicRouteWeb.Server.Exceptions;
using GalacticPOI = EDScenicRouteWeb.Server.Models.GalacticPOI;
using GalacticSystem = EDScenicRouteWeb.Server.Models.GalacticSystem;
using IGalacticPoint = EDScenicRouteWeb.Server.Models.IGalacticPoint;
using ScenicSuggestion = EDScenicRouteWeb.Server.Models.ScenicSuggestion;
using ScenicSuggestionResults = EDScenicRouteWeb.Server.Models.ScenicSuggestionResults;

namespace EDScenicRouteWeb.Server.Services
{
    public class ScenicSuggestionCalculator
    {
        private const int MAX_SUGGESTIONS = 500;
        private readonly float bubbleIgnoreRadius;

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
            if (DistanceFromSol(to.Coordinates) < bubbleIgnoreRadius && DistanceFromSol(from.Coordinates) < bubbleIgnoreRadius)
            {
                // Warn user that their trip is very close to Earth and we won't have any useful POI suggestions!
                throw new TripWithinBubbleException();
            }
            var originalDistance = DistanceBetweenPoints(from, to);
            var edsmSuggestions = SuggestionsForList(POIs.Where(p => (int)p.Type < 100));
            var codexSuggestions = SuggestionsForList(POIs.Where(p => (int) p.Type >= 100));
            
            return new ScenicSuggestionResults()
            {
                StraightLineDistance = originalDistance,
                Suggestions = edsmSuggestions.Concat(codexSuggestions).ToList()
            };
            
            List<ScenicSuggestion> SuggestionsForList(IEnumerable<GalacticPOI> pois)
            {
                return pois.
                    Where(p => p.DistanceFromSol > bubbleIgnoreRadius). // Ignore the "bubble" of near-Earth POIs
                    Select(p =>
                    {
                        var result = ExtraDistanceIncurred(@from, to, p, originalDistance);
                        return new ScenicSuggestion(p, result.extraDistance, result.percentageAlongRoute);
                    }).
                    Where(ss => ss.ExtraDistance <= acceptableExtraDistance && ss.ExtraDistance > 0f).
                    OrderBy(ss => ss.ExtraDistance).
                    Take(MAX_SUGGESTIONS / 2).
                    ToList();
            }
        }

        public static float DistanceFromSol(System.Numerics.Vector3 point) => point.Length();

        private static (float extraDistance, float percentageAlongRoute) ExtraDistanceIncurred(
            GalacticSystem systemA, GalacticSystem systemB, GalacticPOI poi, float originalDistance)
        {
            var a = systemA.Coordinates;
            var b = systemB.Coordinates;
            var p = poi.Coordinates;

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
            return System.Numerics.Vector3.Distance(a.Coordinates, b.Coordinates);
        }
    }
}
