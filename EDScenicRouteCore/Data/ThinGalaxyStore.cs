using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            var system = Systems.FirstOrDefault(s => s.Name == name);
            if (system == null)
            {
                system = await systemEnquirer.GetSystemAsync(name);
                if (systems.Count >= MAX_CACHED_SYSTEMS)
                {
                    systems[random.Next(MAX_CACHED_SYSTEMS)] = system;
                }
                else
                {
                    systems.Add(system);
                }
            }

            return system;
        }

    }
}
