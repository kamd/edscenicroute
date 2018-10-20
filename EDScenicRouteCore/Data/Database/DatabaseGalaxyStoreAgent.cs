using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCore.DataUpdates;
using EDScenicRouteCoreModels;
using Microsoft.EntityFrameworkCore;

namespace EDScenicRouteCore.Data.Database
{
    internal class DatabaseGalaxyStoreAgent : IGalaxyStoreAgent
    {
        private readonly GalacticSystemContext context;

        public DatabaseGalaxyStoreAgent(GalacticSystemContext galacticSystemContext, IEnumerable<GalacticPOI> pois)
        {
            context = galacticSystemContext;
            POIs = pois;
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

        public Task<GalacticSystem> ResolveSystemByName(string name)
        {
            var system = Systems.FirstOrDefault(s => s.Name == name);
            if (system == null)
            {
                throw new SystemNotFoundException() { SystemName = name };
            }
            return Task.FromResult(system);
        }

        public IQueryable<GalacticSystem> Systems => context.GalacticSystems;
        public IEnumerable<GalacticPOI> POIs { get; }
    }
}
