﻿using EDScenicRouteCore.Data;
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
            var storeType = config.GetSection("GalaxyStore")["StoreType"];
            BackingStoreType dbType;
            switch (storeType)
            {
                case "Thin":
                    dbType = BackingStoreType.Thin;
                    break;
                case "PostgreSQL":
                    dbType = BackingStoreType.PostgreSQL;
                    break;
                default:
                    throw new ArgumentException("Wrong backing type in configuration.");
            }
            if (dbType == BackingStoreType.PostgreSQL)
            {
                galaxyStore = new DatabaseGalaxyStore(config, logger, dbType);
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