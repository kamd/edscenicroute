using EDScenicRouteCore.Data;
using EDScenicRouteCore.DataUpdates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore
{
    public class Galaxy
    {
        private readonly string DefaultPOIFilePath = Path.GetTempPath() + @"pois.xml";
        private readonly string DefaultSystemsFilePath = Path.GetTempPath() + @"systems.xml";
        private readonly EDSMSystemEnquirer systemEnquirer = new EDSMSystemEnquirer();
        private ScenicSuggestionCalculator calculator;
        private bool initialised;

        private static readonly object saveLock = new object();

        public List<GalacticPOI> POIs { get; private set; }

        public List<GalacticSystem> Systems { get; private set; }

        public async Task Initialise()
        {
            if (initialised)
            {
                throw new InvalidOperationException("Already initialised.");
            }
            await LoadPOIs();
            LoadSystems();
            calculator = new ScenicSuggestionCalculator(POIs, Systems);
            initialised = true;
        }

        private async Task LoadPOIs()
        {
            if (File.Exists(DefaultPOIFilePath)) {
                POIs = GalacticPOISerialization.LoadFromFile(DefaultPOIFilePath);
            } else {
                var downloader = new EDSMDownloader();
                var json = await downloader.DownloadPOIInfoAsJSON();
                var converter = new JSONToPOIConverter();
                POIs = converter.ConvertJSONToPOIs(json);
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
                Systems = GalacticSystemSerialization.LoadFromFile(DefaultSystemsFilePath);
            }
            else
            {
                Systems = new List<GalacticSystem>();
            }
        }

        public void SaveSystems()
        {
            lock (saveLock)
            {
                GalacticSystemSerialization.SaveToFile(Systems, DefaultSystemsFilePath);
            }
        }

        public ScenicSuggestionResults GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            CheckInitialised();
            return calculator.GenerateSuggestions(from, to, acceptableExtraDistance);
        }

        public async Task<ScenicSuggestionResults> GenerateSuggestions(RouteDetails details)
        {
            return await GenerateSuggestions(details.FromSystemName, details.ToSystemName, details.AcceptableExtraDistance);
        }

        public async Task<ScenicSuggestionResults> GenerateSuggestions(
            string placeNameFrom,
            string placeNameTo,
            float acceptableExtraDistance)
        {
            CheckInitialised();
            var systemFrom = await ResolvePlaceByName(placeNameFrom);
            var systemTo = await ResolvePlaceByName(placeNameTo);
            return GenerateSuggestions(systemFrom, systemTo, acceptableExtraDistance);
        }

        private async Task<GalacticSystem> ResolvePlaceByName(string name)
        {
            var poi = POIs.FirstOrDefault(p => p.Name == name);
            if (poi != null)
            {
                return await ResolveSystemByName(poi.GalMapSearch);
            }

            return await ResolveSystemByName(name);
        }

        private async Task<GalacticSystem> ResolveSystemByName(string name)
        {
            var system = Systems.FirstOrDefault(s => s.Name == name);
            if (system == null)
            {
                system = await systemEnquirer.GetSystemAsync(name);
                Systems.Add(system);
            }

            return system;
        }

        private void CheckInitialised()
        {
            if (!initialised)
            {
                throw new InvalidOperationException("Not yet initialised!");
            }
        }
    }
}