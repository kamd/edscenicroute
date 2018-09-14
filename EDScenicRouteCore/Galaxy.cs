using EDScenicRouteCore.Data;
using EDScenicRouteCore.DataUpdates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDScenicRouteCore
{
    public class Galaxy
    {

        
        private ScenicSuggestionCalculator calculator;
        private IGalaxyStore galaxyStore;
        private bool initialised;
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public Galaxy(IConfiguration configuration, ILogger logWriter)
        {
            config = configuration;
            logger = logWriter;
        }

        public IQueryable<GalacticPOI> POIs => galaxyStore.POIs;

        public IQueryable<GalacticSystem> Systems => galaxyStore.Systems;


        public Task Initialise(CancellationToken cancellationToken)
        {
            if (initialised)
            {
                throw new InvalidOperationException("Already initialised.");
            }
            var updateDir = config.GetSection("GalaxyStore")["StoreType"];
            if (!string.IsNullOrEmpty(updateDir) && updateDir == "SQLite")
            {
                galaxyStore = new DatabaseGalaxyStore(config, logger);
            }
            else
            {
                galaxyStore = new ThinGalaxyStore(config);
            }

            galaxyStore.UpdateFromLocalFiles(cancellationToken);

            calculator = new ScenicSuggestionCalculator(POIs, Systems);
            initialised = true;

            return Task.CompletedTask;
        }


        public ScenicSuggestionResults GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            CheckInitialised();
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
            CheckInitialised();
            var systemFrom = await galaxyStore.ResolvePlaceByName(placeNameFrom);
            var systemTo = await galaxyStore.ResolvePlaceByName(placeNameTo);
            return GenerateSuggestions(systemFrom, systemTo, acceptableExtraDistance);
        }


        public void SaveSystems()
        {
            galaxyStore.SaveSystems();
        }

        private void CheckInitialised()
        {
            if (!initialised)
            {
                throw new InvalidOperationException("Not yet initialised!");
            }
        }

        public void UpdateSystemsFromFile(string s, CancellationToken cancellationToken)
        {
            galaxyStore.UpdateSystemsFromFile(s, cancellationToken);
        }

        public void UpdatePOIsFromFile(string s)
        {
            galaxyStore.UpdatePOIsFromFile(s);
        }
    }
}