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
        private readonly Galaxy galaxy;

        public GalaxyManager(IConfiguration configuration, ILogger<GalaxyManager> logger)
        {
            galaxy = new Galaxy(configuration, logger);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await galaxy.Initialise(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            galaxy.SaveSystems();
            return Task.CompletedTask;
        }

        public EDScenicRouteCore.GalaxyAgent GetAgent()
        {
            return galaxy.GetAgent();
        }



        private ILogger<GalaxyManager> Logger { get; }
    }
}
