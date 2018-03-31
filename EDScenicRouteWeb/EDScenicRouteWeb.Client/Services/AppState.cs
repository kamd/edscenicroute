using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;
using Microsoft.AspNetCore.Blazor;

namespace EDScenicRouteWeb.Client.Services
{
    public class AppState
    {
        // Actual state
        public float StraightLineDistanceOfTrip { get; private set; }
        public IReadOnlyList<ScenicSuggestion> Suggestions { get; private set; } = new List<ScenicSuggestion>();
        public bool CurrentlySearching { get; private set; }
        public string DebugString { get; set; } = "nonono...";

        // Lets components receive change notifications
        // Could have whatever granularity you want (more events, hierarchy...)
        public event Action OnChanged;

        // Receive 'http' instance from DI
        private readonly HttpClient http;
        public AppState(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        public async Task ChangeDebugString()
        {
            await Task.Delay(1000);
            DebugString = "YESYESYES";
            NotifyStateChanged();
        }

        public async Task GetSuggestions(RouteDetails details)
        {
            CurrentlySearching = true;
            NotifyStateChanged();

            var results = await http.PostJsonAsync<ScenicSuggestionResults>("/api/scenicsuggestions", details);
            StraightLineDistanceOfTrip = results.StraightLineDistance;
            Suggestions = results.Suggestions;
            DebugString = "Yes yes yes!";
            CurrentlySearching = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChanged?.Invoke();
        }
    }
}
