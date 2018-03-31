using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;

namespace EDScenicRouteWeb.Server.Services
{
    public interface IGalaxyManager
    {
        Task Initialise();
        Task<ScenicSuggestionResults> GenerateSuggestions(RouteDetails details);
    }
}
