using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Data;
using EDScenicRouteWeb.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EDScenicRouteWeb.Server.Repositories
{
    public class PoiCache
    {
        private List<GalacticPOI> pois;
        private DateTime lastRefreshTime;

        public async Task<List<GalacticPOI>> GetPOIs(GalacticSystemContext context)
        {
            if (pois == null || (DateTime.UtcNow - lastRefreshTime) >= new TimeSpan(1, 0, 0))
            {
                pois = await context.GalacticPOIs.ToListAsync();
                lastRefreshTime = DateTime.UtcNow;
            }

            return pois;
        }
    }
}