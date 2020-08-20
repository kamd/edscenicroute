using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EDScenicRouteWeb.Server.Data.DataUpdates.Database
{
    public static class DatabaseUpdater
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
