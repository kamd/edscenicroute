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

        // Lets components receive change notifications
        // Could have whatever granularity you want (more events, hierarchy...)
        public event Action OnChanged;

        // Receive 'http' instance from DI
        private readonly HttpClient http;
        public AppState(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        public async Task GetSuggestions(RouteDetails details)
        {
            CurrentlySearching = true;
            NotifyStateChanged();

            (StraightLineDistanceOfTrip, Suggestions) = (11f, await http.PostJsonAsync<ScenicSuggestion[]>("/api/scenicsuggestions", details));
            CurrentlySearching = false;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChanged?.Invoke();
        }
    }
}
