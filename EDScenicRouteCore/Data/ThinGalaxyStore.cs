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

namespace EDScenicRouteCore.Data
{
    public class ThinGalaxyStore : IGalaxyStore
    {
        private readonly EDSMSystemEnquirer systemEnquirer = new EDSMSystemEnquirer();
        private static readonly object saveLock = new object();
        private readonly string DefaultPOIFilePath = Path.GetTempPath() + @"pois.xml";
        private readonly string DefaultSystemsFilePath = Path.GetTempPath() + @"systems.xml";
        private List<GalacticSystem> systems;
        private List<GalacticPOI> pois;

        public IQueryable<GalacticSystem> Systems => systems.AsQueryable();

        public IQueryable<GalacticPOI> POIs => pois.AsQueryable();

        public ThinGalaxyStore(IConfiguration configuration)
        {
            
        }

        public void UpdateFromLocalFiles(CancellationToken cancellationToken)
        {
            LoadPOIs().Wait(cancellationToken);
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
                systems = GalacticSystemSerialization.LoadFromFile(DefaultSystemsFilePath);
            }
            else
            {
                systems = new List<GalacticSystem>();
            }
        }

        public void SaveSystems()
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
                systems.Add(system);
            }

            return system;
        }

        public void UpdateSystemsFromFile(string s, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void UpdatePOIsFromFile(string s)
        {
            throw new NotImplementedException();
        }
    }
}
