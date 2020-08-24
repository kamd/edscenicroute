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

        private readonly IGalaxyRepository repository;

        public GalaxyService(IGalaxyRepository galaxyRepository)
        {
            repository = galaxyRepository;
        }

        public async Task<ScenicSuggestionResults> GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            var calculator = new ScenicSuggestionCalculator(await repository.GetPOIs(), repository.Systems, BUBBLE_IGNORE_RADIUS);
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
            return await GenerateSuggestions(systemFrom, systemTo, acceptableExtraDistance);
        }

        public async Task<List<string>> PlaceNamesContainingString(string input)
        {
            string likeInput = $"%{input}%";

            var pois = (await repository.GetPOIs())
                .Where(p => (int)p.Type < 100 && p.Name.ToLower().Contains(input))
                .Select(p => p.Name)
                .OrderBy(p => p)
                .Take(PLACE_NAME_RESULTS_COUNT);

            var systems = await repository.Systems
                .Where(s => EF.Functions.ILike(s.Name, likeInput))
                .Select(s => s.Name)
                .Take(PLACE_NAME_RESULTS_COUNT)
                .ToListAsync();

            return pois
                .Concat(systems)
                .Distinct()
                .Take(PLACE_NAME_RESULTS_COUNT)
                .ToList();
        }

    }
}
