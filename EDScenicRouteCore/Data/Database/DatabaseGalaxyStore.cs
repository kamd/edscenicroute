using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCore.Data.Database;
using EDScenicRouteCore.DataUpdates;
using EDScenicRouteCoreModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDScenicRouteCore.Data
{
    public class DatabaseGalaxyStore : IGalaxyStore
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;
        private readonly DbContextOptionsBuilder<GalacticSystemContext> optionsBuilder;
        private readonly IEnumerable<GalacticPOI> pois;
        private readonly GalacticSystemContext context;

        public DatabaseGalaxyStore(IConfiguration configuration, ILogger logWriter)
        {
            config = configuration;
            logger = logWriter;
            logger.Log(LogLevel.Information, "Using SQLite backing store.");
            optionsBuilder = new DbContextOptionsBuilder<GalacticSystemContext>();
            optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            context = new GalacticSystemContext(optionsBuilder.Options);
            
            // We need to examine every POI for each query, so read and cache the entire collection.
            pois = context.GalacticPOIs.Include(x => x.Coordinates).ToList();
        }

        public async Task UpdateFromLocalFiles(CancellationToken cancellationToken)
        {
            
            await Task.Run(() => ConsumeJSONFiles("POIUpdateDirectory", UpdatePOIsFromFile));
            await Task.Run(() => ConsumeJSONFiles("SystemUpdateDirectory", x => UpdateSystemsFromFile(x, cancellationToken)));

            void ConsumeJSONFiles(string configKey, Action<string> consumeAction)
            {
                var updateDir = config.GetSection("GalaxyUpdates")[configKey];
                if (!string.IsNullOrEmpty(updateDir))
                {
                    logger.Log(LogLevel.Information, $"Checking {configKey} {updateDir}...");
                    foreach (var jsonFile in Directory.GetFiles(updateDir, "*.json"))
                    {
                        consumeAction(jsonFile);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }
                        File.Delete(jsonFile);
                    }
                }
            }
        }

        IGalaxyStoreAgent IGalaxyStore.GetAgent()
        {
            var context = new GalacticSystemContext(optionsBuilder.Options);
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            return new DatabaseGalaxyStoreAgent(context, pois);
        }
        

        private void UpdateSystemsFromFile(string filename, CancellationToken cancellationToken)
        {
            DatabaseUpdater.UpdateSystemsFromFile(context, filename, cancellationToken, logger);
        }

        private void UpdatePOIsFromFile(string filename)
        {
            DatabaseUpdater.UpdatePOIsFromFile(context, filename, logger);
        }

        public void Save()
        {
            ;
        }

    }
}
