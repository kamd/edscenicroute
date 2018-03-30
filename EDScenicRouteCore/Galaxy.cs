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

        public Galaxy()
        {
            
        }

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
                GalacticPOISerialization.SaveToFile(POIs, DefaultPOIFilePath);
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
            GalacticSystemSerialization.SaveToFile(Systems, DefaultSystemsFilePath);
        }

        public (float, List<ScenicSuggestion>) GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance)
        {
            CheckInitialised();
            return calculator.GenerateSuggestions(from, to, acceptableExtraDistance);
        }

        public async Task<(float, List<ScenicSuggestion>)> GenerateSuggestions(RouteDetails details)
        {
            return await GenerateSuggestions(details.FromSystemName, details.ToSystemName, details.AcceptableExtraDistance);
        }

        public async Task<(float, List<ScenicSuggestion>)> GenerateSuggestions(
            string systemNameFrom,
            string systemNameTo,
            float acceptableExtraDistance)
        {
            CheckInitialised();
            var systemFrom = await ResolveSystemByName(systemNameFrom);
            var systemTo = await ResolveSystemByName(systemNameTo);
            return GenerateSuggestions(systemFrom, systemTo, acceptableExtraDistance);
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