using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Repositories;
using Microsoft.EntityFrameworkCore;
using GalacticSystem = EDScenicRouteWeb.Server.Models.GalacticSystem;
using RouteDetails = EDScenicRouteWeb.Server.Models.RouteDetails;
using ScenicSuggestionResults = EDScenicRouteWeb.Server.Models.ScenicSuggestionResults;

namespace EDScenicRouteWeb.Server.Services
{
    public class GalaxyService : IGalaxyService
    {
        private const int PLACE_NAME_RESULTS_COUNT = 6;
        private const float BUBBLE_IGNORE_RADIUS = 200f;

        private readonly ScenicSuggestionCalculator calculator;
        private readonly IGalaxyRepository repository;

        public GalaxyService(IGalaxyRepository galaxyRepository)
        {
            calculator = new ScenicSuggestionCalculator(galaxyRepository.POIs, galaxyRepository.Systems, BUBBLE_IGNORE_RADIUS);
            repository = galaxyRepository;
        }

        public ScenicSuggestionResults GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            return calculator.GenerateSuggestions(from, to, acceptableExtraDistance);
        }

        public async Task<ScenicSuggestionResults> GenerateSuggestions(RouteDetails details)
        {
            return await GenerateSuggestions(details.FromSystemName, details.ToSystemName, details.AcceptableExtraDistance);
        }

        public async Task<ScenicSuggestionResults> GenerateSuggestions(
            string placeNameFrom,
            string placeNameTo,
            float acceptableExtraDistance)
        {
            var systemFrom = await repository.ResolvePlaceByName(placeNameFrom);
            var systemTo = await repository.ResolvePlaceByName(placeNameTo);
            return GenerateSuggestions(systemFrom, systemTo, acceptableExtraDistance);
        }

        public async Task<List<string>> PlaceNamesContainingString(string input)
        {
            string likeInput = $"%{input}%";
            return await Task.Run(() =>
            {
                var pois = repository.POIs.Where(p => p.Name.ToLower().
                    Contains(input)).
                    Select(p => p.Name).
                    OrderBy(p => p)
                    .Take(PLACE_NAME_RESULTS_COUNT);

                var systems = repository.Systems.Where(s => EF.Functions.ILike(s.Name, likeInput)).
                    Select(s => s.Name).
                    OrderBy(s => s).
                    Take(PLACE_NAME_RESULTS_COUNT);

                return pois.Concat(systems).Distinct().Take(PLACE_NAME_RESULTS_COUNT).ToList();
            });
                
        }

    }
}
