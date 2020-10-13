using System;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Exceptions;
using EDScenicRouteWeb.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RouteDetails = EDScenicRouteWeb.Server.Models.RouteDetails;
using RouteDetailsValidator = EDScenicRouteWeb.Server.Services.RouteDetailsValidator;

namespace EDScenicRouteWeb.Server.Controllers
{
    [Route("api/scenicsuggestions")]
    public class ScenicSuggestionsController : Controller
    {
        private readonly IGalaxyService galaxy;

        public ScenicSuggestionsController(IGalaxyService galaxyService, ILogger<ScenicSuggestionsController> logger)
        {
            galaxy = galaxyService;
            Logger = logger;
        }

        private ILogger<ScenicSuggestionsController> Logger { get; }

        [HttpPost]
        public async Task<IActionResult> GetSuggestions([FromBody] RouteDetails details)
        {
            (bool success, string errorMessage) = RouteDetailsValidator.ValidateRouteDetails(details);
            if (!success)
            {
                Logger.LogWarning(LoggingEvents.BadRouteDetails, errorMessage, DateTime.Now);
                return BadRequest(errorMessage);
            }

            Logger.LogInformation(LoggingEvents.GetSuggestions,
                $"{DateTime.Now} [{details.FromSystemName}] - [{details.ToSystemName}] : {details.AcceptableExtraDistance}");
            try
            {
                var results = await galaxy.GenerateSuggestions(details);
                Logger.LogInformation(LoggingEvents.GetSuggestionsSuccess,
                    $"{DateTime.Now} POIs found: {results.Suggestions.Count}");
                return Ok(results);
            }
            catch (SystemNotFoundException systemNotFoundException)
            {
                Logger.LogWarning(LoggingEvents.SystemNotFound,
                    $"{DateTime.Now} Name: {systemNotFoundException.SystemName}");
                return NotFound($"System '{systemNotFoundException.SystemName}' was not found in the galaxy.");
            }
            catch (TripWithinBubbleException)
            {
                Logger.LogWarning(LoggingEvents.TripWithinBubble,
                    $"{DateTime.Now} Trip within bubble.");
                return NotFound("Your trip is entirely within the 'bubble' of near-Earth systems, whose nearby POIs are excluded from this search."
                + " Try setting your sights further afield!");
            }
            catch (OperationCanceledException)
            {
                Logger.LogError(LoggingEvents.Timeout, $"{DateTime.Now} Timeout in GetSuggestions");
                return NotFound("Elite Dangerous Star Map could not be consulted, please try again later.");
            }
            catch (GalaxyNotInitialisedException)
            {
                Logger.LogError(LoggingEvents.NotYetInitialised, $"{DateTime.Now} Request received before galaxy initialised!");
                return NotFound("Galaxy data server is busy initialising, please try again later.");
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.UnknownError, ex, $"{DateTime.Now} Error in GetSuggestions");
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
            public const int Timeout = 4003;
            public const int NotYetInitialised = 4004;
            public const int TripWithinBubble = 4005;
        }
    }
}
