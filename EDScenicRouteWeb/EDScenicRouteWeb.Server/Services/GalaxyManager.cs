using EDScenicRouteCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;
using Microsoft.Extensions.Hosting;

namespace EDScenicRouteWeb.Server.Services
{
    public class GalaxyManager : IGalaxyManager, IHostedService
    {

        private const int AUTOCOMPLETE_RESULTS = 6;
        private readonly TimeSpan SAVE_DELAY = new TimeSpan(0, 5, 0);

        public Galaxy EDGalaxy { get; set; } = new Galaxy();

        public async Task Initialise()
        {
            await EDGalaxy.Initialise();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Initialise();
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(SAVE_DELAY, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    return;
                }

                EDGalaxy.SaveSystems();
            }
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
                    Take(AUTOCOMPLETE_RESULTS).
                    ToList());
        }
    }
}
