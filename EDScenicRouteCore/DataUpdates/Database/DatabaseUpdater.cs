using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCore.Data;
using EDScenicRouteCoreModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq;

namespace EDScenicRouteCore.DataUpdates
{
    public class DatabaseUpdater
    {
        public static void UpdatePOIsFromFile(GalacticSystemContext context, string filename, ILogger logger)
        {
            logger.Log(LogLevel.Information, $"Updating POIs from {filename}...");
            // The total number of POIs in Elite Dangerous is low enough to add all at once.
            var jsonConverter = new JSONToGalacticPointConverter();
            var pois = jsonConverter.ConvertJSONToPOIs(filename)
                .Where(a => !context.GalacticPOIs.Any(b => b.Id == a.Id)).ToImmutableList();
            logger.Log(LogLevel.Debug, $"{pois.Count} new POIs in this file.");
            context.GalacticPOIs.AddRange(pois);
            context.SaveChanges();
            logger.Log(LogLevel.Information, $"Done updating POIs from {filename}.");
        }

        public static void UpdateSystemsFromFile(GalacticSystemContext context, string filename, CancellationToken cancellationToken, ILogger logger)
        {
            var jsonConverter = new JSONToGalacticPointConverter();
            logger.Log(LogLevel.Information, $"Updating systems from {filename}...");
            var systemsFromFile = jsonConverter.ConvertJSONToSystems(filename);

            long i = 0;
            int x = 10000;
            foreach (var batch in systemsFromFile.Batch(x))
            {
                var systemsInBatch = batch.ToList();
                var newSystems = systemsInBatch.Where(a => !context.GalacticSystems.Any(b => b.Id == a.Id)).ToList();
                var updatedSystems = systemsInBatch.Except(newSystems).ToList();

                logger.Log(LogLevel.Debug, $"{updatedSystems.Count} updated systems in this batch of {x}");
                context.GalacticSystems.UpdateRange(updatedSystems);

                logger.Log(LogLevel.Debug, $"{newSystems.Count} new systems in this batch of {x}");
                context.GalacticSystems.AddRange(newSystems);

                context.SaveChanges();
                i += x;
                logger.Log(LogLevel.Debug, $"Processed {i} systems from file.");
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.Log(LogLevel.Information, "Update systems cancelled.");
                    return;
                }
            }
            logger.Log(LogLevel.Information, $"Done updating systems from {filename}.");
        }
    }
}
