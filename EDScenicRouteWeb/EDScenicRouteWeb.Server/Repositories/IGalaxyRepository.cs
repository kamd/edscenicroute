using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalacticPOI = EDScenicRouteWeb.Server.Models.GalacticPOI;
using GalacticSystem = EDScenicRouteWeb.Server.Models.GalacticSystem;

namespace EDScenicRouteWeb.Server.Repositories
{
    public interface IGalaxyRepository
    {
        Task<GalacticSystem> ResolvePlaceByName(string name);
        Task<GalacticSystem> ResolveSystemByName(string name);
        IQueryable<GalacticSystem> Systems { get; }
        Task<IEnumerable<GalacticPOI>> GetPOIs();
    }
}
