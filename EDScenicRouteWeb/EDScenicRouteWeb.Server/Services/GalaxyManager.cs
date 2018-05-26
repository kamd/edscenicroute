using EDScenicRouteCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteWeb.Server.Services
{
    public class GalaxyManager : IGalaxyManager
    {

        private const int AUTOCOMPLETE_RESULTS = 6;

        public GalaxyManager()
        {
            Initialise().Wait(); //TODO Dodgy
        }

        public Galaxy EDGalaxy { get; set; }

        public async Task Initialise()
        {
            EDGalaxy = new Galaxy();
            await EDGalaxy.Initialise();
        }

        public async Task<ScenicSuggestionResults> GenerateSuggestions(RouteDetails details)
        {
            var results = await EDGalaxy.GenerateSuggestions(details);
            EDGalaxy.SaveSystems(); //TODO: Put this somewhere more sensible
            return results;
        }

        public async Task<List<string>> AutocompletePOINames(string input)
        {
            input = input.ToLower();
            return await Task.Run(() =>
                EDGalaxy.POIs.Where(p => p.Name.ToLower().Contains(input)).Select(p => p.Name).Take(AUTOCOMPLETE_RESULTS)
                    .ToList());
        }
    }
}
