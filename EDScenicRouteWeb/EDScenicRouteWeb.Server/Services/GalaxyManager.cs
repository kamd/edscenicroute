using EDScenicRouteCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDScenicRouteWeb.Server.Services
{
    public class GalaxyManager : IGalaxyManager, IHostedService
    {
        public GalaxyManager(IConfiguration configuration, ILogger<GalaxyManager> logger)
        {
            EDGalaxy = new Galaxy(configuration, logger);
        }

        private const int AUTOCOMPLETE_RESULTS = 6;

        public Galaxy EDGalaxy { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await EDGalaxy.Initialise(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            EDGalaxy.SaveSystems();
            return Task.CompletedTask;
        }

        public async Task<ScenicSuggestionResults> GenerateSuggestions(RouteDetails details)
        {
            var results = await EDGalaxy.GenerateSuggestions(details);
            return results;
        }

        public async Task<List<string>> AutocompletePOINames(string input)
        {
            input = input.ToLower();
            return await Task.Run(() =>
                EDGalaxy.POIs.
                    Where(p => p.Name.ToLower().Contains(input)).
                    Select(p => p.Name).
                    Concat(
                        EDGalaxy.Systems.
                            Where(p => p.Name.ToLower().Contains(input)).
                            Select(p => p.Name)).
                    Take(AUTOCOMPLETE_RESULTS).
                    ToList());
        }

        private ILogger<GalaxyManager> Logger { get; }
    }
}
