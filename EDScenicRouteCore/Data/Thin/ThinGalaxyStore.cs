using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EDScenicRouteCore.Data.Thin;
using EDScenicRouteCore.DataUpdates;
using EDScenicRouteCoreModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDScenicRouteCore.Data
{
    public class ThinGalaxyStore : IGalaxyStore
    {

        private const int MAX_CACHED_SYSTEMS = 1000;
        private readonly EDSMSystemEnquirer systemEnquirer = new EDSMSystemEnquirer();
        private static readonly object saveLock = new object();
        private readonly string DefaultPOIFilePath = Path.GetTempPath() + @"pois.xml";
        private readonly string DefaultSystemsFilePath = Path.GetTempPath() + @"systems.xml";
        private readonly List<GalacticSystem> systems;
        private List<GalacticPOI> pois;
        private readonly Random random = new Random();

        public IQueryable<GalacticSystem> Systems => systems.AsQueryable();

        public IQueryable<GalacticPOI> POIs => pois.AsQueryable();

        public ThinGalaxyStore(IConfiguration configuration, ILogger logger)
        {
            systems = new List<GalacticSystem>(MAX_CACHED_SYSTEMS);
            logger.Log(LogLevel.Information, $"Using thin backing store. (Passthrough with cache of {MAX_CACHED_SYSTEMS})");
        }

        public async Task UpdateFromLocalFiles(CancellationToken cancellationToken)
        {
            await LoadPOIs();
            LoadSystems();
        }

        private async Task LoadPOIs()
        {
            if (File.Exists(DefaultPOIFilePath))
            {
                pois = GalacticPOISerialization.LoadFromFile(DefaultPOIFilePath);
            }
            else
            {
                var downloader = new EDSMPOIDownloader();
                var filename = Path.GetTempFileName();
                await downloader.DownloadPOIInfoAsJSON(filename);
                var converter = new JSONToGalacticPointConverter();
                pois = converter.ConvertJSONToPOIs(filename).ToList();

                lock (saveLock)
                {
                    GalacticPOISerialization.SaveToFile(POIs, DefaultPOIFilePath);
                }
            }
        }

        IGalaxyStoreAgent IGalaxyStore.GetAgent()
        {
            return new ThinGalaxyStoreAgent(systems, pois, random, MAX_CACHED_SYSTEMS);
        }

        private void LoadSystems()
        {
            if (File.Exists(DefaultSystemsFilePath))
            {
                systems.AddRange(GalacticSystemSerialization.LoadFromFile(DefaultSystemsFilePath));
            }
        }

        public void Save()
        {
            lock (saveLock)
            {
                GalacticSystemSerialization.SaveToFile(Systems, DefaultSystemsFilePath);
            }
        }

        

    }
}
