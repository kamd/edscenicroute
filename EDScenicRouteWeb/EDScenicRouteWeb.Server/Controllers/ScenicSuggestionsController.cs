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
using EDScenicRouteWeb.Shared.DataValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;

namespace EDScenicRouteWeb.Server.Controllers
{
    [Route("api/[controller]")]
    public class ScenicSuggestionsController : Controller
    {

        public ScenicSuggestionsController(IGalaxyManager galaxyManager, ILogger<ScenicSuggestionsController> logger)
        {
            Galaxy = galaxyManager;
            Logger = logger;
        }

        private IGalaxyManager Galaxy { get; }

        private ILogger<ScenicSuggestionsController> Logger { get; }

        [HttpPost]
        public async Task<IActionResult> GetSuggestions([FromBody] RouteDetails details)
        {
            (bool success, string errorMessage) = RouteDetailsValidator.ValidateRouteDetails(details);
            if (!success)
            {
                Logger.LogWarning(LoggingEvents.BadRouteDetails, errorMessage);
                return BadRequest(errorMessage);
            }
            Logger.LogInformation(LoggingEvents.GetSuggestions,
                $"[{details.FromSystemName}] - [{details.ToSystemName}] : {details.AcceptableExtraDistance}");
            try
            {
                var results = await Galaxy.GenerateSuggestions(details);
                Logger.LogInformation(LoggingEvents.GetSuggestionsSuccess, $"POIs found: {results.Suggestions.Count}");
                return Ok(results);
            }
            catch (SystemNotFoundException systemNotFoundException)
            {
                Logger.LogWarning(LoggingEvents.SystemNotFound, $"Name: {systemNotFoundException.SystemName}");
                return NotFound($"System '{systemNotFoundException.SystemName}' was not found in the galaxy.");
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.UnknownError, ex, "Error in GetSuggestions");
                throw;
            }
        }

        public class LoggingEvents
        {
            public const int GetSuggestions = 1000;
            public const int GetSuggestionsSuccess = 1001;

            public const int SystemNotFound = 4000;
            public const int UnknownError = 4001;
            public const int BadRouteDetails = 4002;
        }
    }
}
