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
using EDScenicRouteCore.Exceptions;
using EDScenicRouteCoreModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDScenicRouteCore
{
    public class Galaxy
    {
        private IGalaxyStore galaxyStore;
        private bool initialised;
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public Galaxy(IConfiguration configuration, ILogger logWriter)
        {
            config = configuration;
            logger = logWriter;
        }

        public GalaxyAgent GetAgent()
        {
            CheckInitialised();
            return new GalaxyAgent(galaxyStore.GetAgent());
        }

        public async Task Initialise(CancellationToken cancellationToken)
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
                galaxyStore = new ThinGalaxyStore(config, logger);
            }

            await galaxyStore.UpdateFromLocalFiles(cancellationToken);
            
            initialised = true;
        }

        public void SaveSystems()
        {
            galaxyStore.Save();
        }

        private void CheckInitialised()
        {
            if (!initialised)
            {
                throw new GalaxyNotInitialisedException();
            }
        }

    }
}