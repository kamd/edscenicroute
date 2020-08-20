using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Data;
using EDScenicRouteWeb.Server.Exceptions;
using Microsoft.EntityFrameworkCore;
using GalacticPOI = EDScenicRouteWeb.Server.Models.GalacticPOI;
using GalacticSystem = EDScenicRouteWeb.Server.Models.GalacticSystem;

namespace EDScenicRouteWeb.Server.Repositories
{
    public class DatabaseGalaxyRepository : IGalaxyRepository
    {
        private readonly GalacticSystemContext context;

        public DatabaseGalaxyRepository(GalacticSystemContext galacticSystemContext)
        {
            context = galacticSystemContext;
            POIs = context.GalacticPOIs.AsNoTracking().ToList();
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
