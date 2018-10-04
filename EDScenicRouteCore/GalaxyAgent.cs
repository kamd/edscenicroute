using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCore.Data;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore
{
    public class GalaxyAgent
    {
        private const int PLACE_NAME_RESULTS_COUNT = 6;

        private readonly ScenicSuggestionCalculator calculator;
        private readonly IGalaxyStoreAgent store;

        internal GalaxyAgent(IGalaxyStoreAgent galaxyStore)
        {
            calculator = new ScenicSuggestionCalculator(galaxyStore.POIs, galaxyStore.Systems);
            store = galaxyStore;
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
            var systemFrom = await store.ResolvePlaceByName(placeNameFrom);
            var systemTo = await store.ResolvePlaceByName(placeNameTo);
            return GenerateSuggestions(systemFrom, systemTo, acceptableExtraDistance);
        }

        public async Task<List<string>> PlaceNamesContainingString(string input)
        {
            input = input.ToLower();
            return await Task.Run(() =>
                store.POIs.
                    Where(p => p.Name.ToLower().Contains(input)).
                    Select(p => p.Name).
                    Concat(
                        store.Systems.
                            Where(p => p.Name.ToLower().Contains(input)).
                            Select(p => p.Name)).
                    Take(PLACE_NAME_RESULTS_COUNT).
                    ToList());
        }

    }
}
