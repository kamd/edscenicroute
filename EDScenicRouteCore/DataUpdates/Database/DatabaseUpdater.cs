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
    }
}
