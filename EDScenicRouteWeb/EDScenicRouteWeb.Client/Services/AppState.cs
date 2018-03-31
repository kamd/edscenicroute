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
        public IReadOnlyList<ScenicSuggestion> Suggestions { get; private set; } = new List<ScenicSuggestion>();
        public bool CurrentlySearching { get; private set; }
        public string DebugString { get; set; } = "nonono...";
        public string ErrorMessage { get; set; }
        public event Action OnChanged;

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
            DebugString = "Yes yes yes!";
            CurrentlySearching = false;
            ErrorMessage = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            OnChanged?.Invoke();
        }
    }
}
