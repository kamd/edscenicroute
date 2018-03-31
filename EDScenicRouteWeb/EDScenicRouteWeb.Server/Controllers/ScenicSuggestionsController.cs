using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;
using EDScenicRouteWeb.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDScenicRouteWeb.Server.Controllers
{
   // [Produces("application/json")]
    [Route("api/[controller]")]
    public class ScenicSuggestionsController : Controller
    {

        public ScenicSuggestionsController(IGalaxyManager galaxyManager)
        {
            Galaxy = galaxyManager;
        }

        private IGalaxyManager Galaxy { get; }

        // GET: api/ScenicSuggestions
     /*   [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */
        [HttpPost]
        public async Task<ScenicSuggestionResults> GetSuggestions([FromBody] RouteDetails details)
        {
            //await Task.Delay(500);
            //return new ScenicSuggestionResults() {StraightLineDistance = 123f, Suggestions = TestData};
            try
            {

                return await Galaxy.GenerateSuggestions(details);
            }
            catch (Exception )
            {
                // throw error?
                throw;
            }
            
        }


        public string Hello()
        {
            return "Why hello world!";
        }
        /*
        // GET: api/ScenicSuggestions/5
        [HttpGet("{id}", Name = "Get")]
      //  [HttpGet()]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/ScenicSuggestions
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/ScenicSuggestions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/

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
                    Type = "Nebula"
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
                    Type = "Nebula"
                }, 12f)
        };
    }
}
