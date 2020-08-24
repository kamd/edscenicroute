using System.Collections.Generic;
using System.Threading.Tasks;
using EDScenicRouteWeb.Server.Models;

namespace EDScenicRouteWeb.Server.Services
{
    public interface IGalaxyService
    {
        Task<ScenicSuggestionResults> GenerateSuggestions(
            GalacticSystem from,
            GalacticSystem to,
            float acceptableExtraDistance);

        Task<ScenicSuggestionResults> GenerateSuggestions(RouteDetails details);

        Task<ScenicSuggestionResults> GenerateSuggestions(
            string placeNameFrom,
            string placeNameTo,
            float acceptableExtraDistance);

        Task<List<string>> PlaceNamesContainingString(string input);
    }
}