using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;
using Microsoft.AspNetCore.Blazor;

namespace EDScenicRouteWeb.Client.Services
{
    public class AppState
    {
        
        public float StraightLineDistanceOfTrip { get; private set; }
        public float ShipJumpRange { get; set; } = 30f;
        public IReadOnlyList<ScenicSuggestion> Suggestions { get; private set; } = new List<ScenicSuggestion>();
        public bool CurrentlySearching { get; private set; }
        public string ErrorMessage { get; set; }
        public float AcceptableExtraDistance { get; set; }
        public bool SearchCompleted { get; set; } = false;
        public event Action OnChanged;

        private readonly HttpClient http;

        public AppState(HttpClient httpInstance)
        {
            http = httpInstance;
        }

        public async Task GetSuggestions(RouteDetails details)
        {
            AcceptableExtraDistance = details.AcceptableExtraDistance;
            CurrentlySearching = true;
            NotifyStateChanged();
            SearchCompleted = true;

            var response = await http.PostAsync("/api/scenicsuggestions", new StringContent(JsonUtil.Serialize(details), Encoding.UTF8, "application/json"));
            if (! response.IsSuccessStatusCode)
            {
                ErrorMessage = $"Terrible error: {await response.Content.ReadAsStringAsync()}";
                StraightLineDistanceOfTrip = 0f;
                Suggestions = new List<ScenicSuggestion>();
                CurrentlySearching = false;
                NotifyStateChanged();
                return;
            }
            var results = JsonUtil.Deserialize<ScenicSuggestionResults>(await response.Content.ReadAsStringAsync());
            StraightLineDistanceOfTrip = results.StraightLineDistance;
            Suggestions = results.Suggestions;
            CurrentlySearching = false;
            
            ErrorMessage = null;
            NotifyStateChanged();
        }

        public async Task<List<string>> GetPOITypeaheadSuggestions(string input)
        {
            var response = await http.GetAsync($"/api/poitypeahead/{input}");
            if (!response.IsSuccessStatusCode)
            {
                return new List<string>();
            }

            var results = JsonUtil.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
            return results;
        }

        private void NotifyStateChanged()
        {
            OnChanged?.Invoke();
        }
    }
}
