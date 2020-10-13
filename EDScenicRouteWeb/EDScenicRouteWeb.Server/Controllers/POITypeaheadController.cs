using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EDScenicRouteWeb.Server.Controllers
{
    [Route("/api/poitypeahead")]
    [ApiController]
    public class POITypeaheadController : Controller
    {
        private readonly IGalaxyService galaxyService;
        
        public POITypeaheadController(IGalaxyService galaxyService)
        {
            this.galaxyService = galaxyService;
        }
        
        [HttpGet("{input}")]
        public async Task<List<string>> Get([FromRoute] string input)
        {
            return await galaxyService.PlaceNamesContainingString(input);
        }
    }
}
