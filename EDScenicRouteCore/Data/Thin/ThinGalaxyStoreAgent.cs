using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCore.DataUpdates;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.Data.Thin
{
    internal class ThinGalaxyStoreAgent : IGalaxyStoreAgent
    {
        private readonly List<GalacticPOI> pois;
        private readonly List<GalacticSystem> systems;
        private readonly EDSMSystemEnquirer systemEnquirer;
        private readonly Random random;
        private readonly int maxSystems;

        public ThinGalaxyStoreAgent(List<GalacticSystem> systems, List<GalacticPOI> pois, Random random, int maxSystems)
        {
            this.systems = systems;
            this.pois = pois;
            this.random = random;
            this.maxSystems = maxSystems;
            systemEnquirer = new EDSMSystemEnquirer();
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
                lock (systems)
                {
                    if (systems.Count >= maxSystems)
                    {
                        systems[random.Next(maxSystems)] = system;
                    }
                    else
                    {
                        systems.Add(system);
                    }
                }
            }

            return system;
        }

        public IQueryable<GalacticSystem> Systems => systems.AsQueryable();
        public IEnumerable<GalacticPOI> POIs => pois;
    }
}
