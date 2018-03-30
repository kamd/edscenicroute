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
        public async Task<IEnumerable<ScenicSuggestion>> GetSuggestions([FromBody] RouteDetails details)
        {
            Console.WriteLine(details.FromSystemName);
            await Task.Delay(500);
            return new List<ScenicSuggestion>() {new ScenicSuggestion(new GalacticPOI(){Name = "Funland"}, 7f)};
          //  return await Galaxy.GenerateSuggestions(details);
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
    }
}
