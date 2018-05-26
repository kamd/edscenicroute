using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EDScenicRouteWeb.Server.Controllers
{
    [Route("api/[controller]")]
    public class POITypeaheadController : Controller
    {
        public POITypeaheadController(IGalaxyManager galaxyManager)
        {
            Galaxy = galaxyManager;
        }

        private IGalaxyManager Galaxy { get; }

        [HttpGet("{input}", Name = "Get")]
        public async Task<List<string>> Get(string input)
        {
            return await Galaxy.AutocompletePOINames(input);
        }
    }
}
