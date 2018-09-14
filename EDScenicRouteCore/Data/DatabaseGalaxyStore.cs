using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public DatabaseGalaxyStore(IConfiguration configuration, ILogger logWriter)
        {
            config = configuration;
            logger = logWriter;
            var optionsBuilder = new DbContextOptionsBuilder<GalacticSystemContext>();
            optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            Context = new GalacticSystemContext(optionsBuilder.Options);
            Context.ChangeTracker.AutoDetectChangesEnabled = false;

            // We need to examine every POI for each query, so read and cache the entire collection.
            POIs = Context.GalacticPOIs.Include(x => x.Coordinates).ToList().AsQueryable();
        }

        public void UpdateFromLocalFiles(CancellationToken cancellationToken)
        {
            
            ConsumeJSONFiles("POIUpdateDirectory", UpdatePOIsFromFile);
            ConsumeJSONFiles("SystemUpdateDirectory", x => UpdateSystemsFromFile(x, cancellationToken));

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

        public IQueryable<GalacticSystem> Systems => Context.GalacticSystems;

        public IQueryable<GalacticPOI> POIs { get; }

        private GalacticSystemContext Context { get; }

        public async Task<GalacticSystem> ResolvePlaceByName(string name)
        {
            var poi = POIs.FirstOrDefault(p => p.Name == name);
            if (poi != null)
            {
                return await ResolveSystemByName(poi.GalMapSearch);
            }

            return await ResolveSystemByName(name);
        }

        public async Task<GalacticSystem> ResolveSystemByName(string name)
        {
            var system = await Systems.FirstOrDefaultAsync(s => s.Name == name);
            if (system == null)
            {
                throw new SystemNotFoundException() {SystemName = name};
            }
            return system;
        }

        public void UpdateSystemsFromFile(string filename, CancellationToken cancellationToken)
        {
            DatabaseUpdater.UpdateSystemsFromFile(Context, filename, cancellationToken, logger);
        }

        public void UpdatePOIsFromFile(string filename)
        {
            DatabaseUpdater.UpdatePOIsFromFile(Context, filename, logger);
        }

        public void SaveSystems()
        {
            Context.SaveChanges(true);
        }

    }
}
