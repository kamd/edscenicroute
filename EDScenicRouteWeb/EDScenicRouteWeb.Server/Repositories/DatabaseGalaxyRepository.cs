using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Data;
using EDScenicRouteWeb.Server.Exceptions;
using GalacticPOI = EDScenicRouteWeb.Server.Models.GalacticPOI;
using GalacticSystem = EDScenicRouteWeb.Server.Models.GalacticSystem;

namespace EDScenicRouteWeb.Server.Repositories
{
    public class DatabaseGalaxyRepository : IGalaxyRepository
    {
        private readonly GalacticSystemContext context;
        private readonly PoiCache poiCache;

        public DatabaseGalaxyRepository(GalacticSystemContext galacticSystemContext, PoiCache poiCache)
        {
            context = galacticSystemContext;
            this.poiCache = poiCache;
        }

        public async Task<GalacticSystem> ResolvePlaceByName(string name)
        {
            var poi = (await GetPOIs()).FirstOrDefault(p => p.Name == name);
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
        public async Task<IEnumerable<GalacticPOI>> GetPOIs() => await poiCache.GetPOIs(context);
    }
}
