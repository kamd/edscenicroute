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
        // With use of indices, no SQL command should take longer than this:
        private const int DATABASE_TIMEOUT_SECONDS = 3;

        private readonly IConfiguration config;
        private readonly ILogger logger;
        private readonly DbContextOptionsBuilder<GalacticSystemContext> optionsBuilder;
        private readonly IEnumerable<GalacticPOI> pois;
        private readonly GalacticSystemContext context;
        private static bool _ensureExists;

        public DatabaseGalaxyStore(IConfiguration configuration, ILogger logWriter, BackingStoreType dbType)
        {
            config = configuration;
            logger = logWriter;
            
            optionsBuilder = new DbContextOptionsBuilder<GalacticSystemContext>();
            
            if (dbType == BackingStoreType.PostgreSQL)
            {
                logger.Log(LogLevel.Information, "Using Postgres backing store.");
                optionsBuilder.UseNpgsql(new Npgsql.NpgsqlConnectionStringBuilder()
                {
                    Host = Environment.GetEnvironmentVariable("DBHOST"),
                    Port = int.Parse(Environment.GetEnvironmentVariable("DBPORT")),
                    Database = Environment.GetEnvironmentVariable("DBDATABASE"),
                    Username = Environment.GetEnvironmentVariable("DBUSER"),
                    Password = Environment.GetEnvironmentVariable("DBPASS")
                }.ToString());
            }
            else
            {
                throw new ArgumentException("Unsupported backing type.");
            }
            
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            context = new GalacticSystemContext(optionsBuilder.Options);

            if (!_ensureExists)
            {
                context.Database.Migrate();
                _ensureExists = true;
            }
            
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
            context.Database.SetCommandTimeout(DATABASE_TIMEOUT_SECONDS);
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
