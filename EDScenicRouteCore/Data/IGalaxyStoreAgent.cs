using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.Data
{
    internal interface IGalaxyStoreAgent
    {
        Task<GalacticSystem> ResolvePlaceByName(string name);
        Task<GalacticSystem> ResolveSystemByName(string name);
        IQueryable<GalacticSystem> Systems { get; }
        IQueryable<GalacticPOI> POIs { get; }
    }
}
