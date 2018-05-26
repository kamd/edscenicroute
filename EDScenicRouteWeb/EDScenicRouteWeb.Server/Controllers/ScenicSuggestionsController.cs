using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EDScenicRouteCore.DataUpdates;
using EDScenicRouteCoreModels;
using EDScenicRouteWeb.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;

namespace EDScenicRouteWeb.Server.Controllers
{
    [Route("api/[controller]")]
    public class ScenicSuggestionsController : Controller
    {

        public ScenicSuggestionsController(IGalaxyManager galaxyManager)
        {
            Galaxy = galaxyManager;
        }

        private IGalaxyManager Galaxy { get; }

        [HttpPost]
        public async Task<IActionResult> GetSuggestions([FromBody] RouteDetails details)
        {
            try
            {
                var results = await Galaxy.GenerateSuggestions(details);
                return Ok(results);
            }
            catch (SystemNotFoundException systemNotFoundException)
            {
                return NotFound($"System '{systemNotFoundException.SystemName}' was not found in the galaxy.");
            }
        }

        private List<ScenicSuggestion> TestData => new List<ScenicSuggestion>()
        {
            new ScenicSuggestion(
                new GalacticPOI()
                {
                    Name = "Funland",
                    Coordinates = new Vector3(1, 2, 3),
                    DistanceFromSol = 1f,
                    GalMapSearch = "asd",
                    GalMapUrl = "dfg",
                    Id = "FUNL",
                    Type = GalacticPOIType.nebula
                }, 7f),
            new ScenicSuggestion(
                new GalacticPOI()
                {
                    Name = "Extraland",
                    Coordinates = new Vector3(2, 3, 3),
                    DistanceFromSol = 10f,
                    GalMapSearch = "aasdsd",
                    GalMapUrl = "dfgdsa",
                    Id = "EXL",
                    Type = GalacticPOIType.nebula
                }, 12f)
        };
    }
}
