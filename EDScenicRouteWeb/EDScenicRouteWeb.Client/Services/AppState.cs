using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EDScenicRouteCoreModels;
using Microsoft.AspNetCore.Blazor;
using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using EDScenicRouteWeb.Shared.DataValidation;

namespace EDScenicRouteWeb.Client.Services
{
    public class AppState
    {
        private const string SHIP_JUMP_RANGE_STORAGE_KEY = "ShipJumpRange";
        private const string EXTRA_DISTANCE_STORAGE_KEY = "AcceptableExtraDistance";

        public IReadOnlyList<ScenicSuggestion> Suggestions { get; private set; } = new List<ScenicSuggestion>();
        public bool CurrentlySearching { get; private set; }
        public string ErrorMessage { get; set; }
        public bool SearchCompleted { get; set; } = false;
        public float StraightLineDistanceOfTrip { get; private set; }

        public float ShipJumpRange
        {
            get => shipJumpRange;
            set
            {
                shipJumpRange = value;
                Storage[SHIP_JUMP_RANGE_STORAGE_KEY] = value.ToString("R", CultureInfo.InvariantCulture);
            }
        }

        public float AcceptableExtraDistance
        {
            get => acceptableExtraDistance;
            set
            {
                acceptableExtraDistance = value;
                Storage[EXTRA_DISTANCE_STORAGE_KEY] = value.ToString("R", CultureInfo.InvariantCulture);
            }
        }
        
        public LocalStorage Storage
        {
            get => storage;
            set
            {
                storage = value;
                LoadValuesFromStorage();
            }
        }

        public event Action OnChanged;

        private readonly HttpClient http;
        private LocalStorage storage;
        private float shipJumpRange = 30f;
        private float acceptableExtraDistance = 150f;

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

            // Validate locally
            var (success, errorMessage) = RouteDetailsValidator.ValidateRouteDetails(details);
            if (!success)
            {
                ErrorMessage = errorMessage;
                StraightLineDistanceOfTrip = 0f;
                Suggestions = new List<ScenicSuggestion>();
                CurrentlySearching = false;
                NotifyStateChanged();
                return;
            }

            // Call API
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

        private void LoadValuesFromStorage()
        {
            var shipJumpRangeStoredValue = Storage[SHIP_JUMP_RANGE_STORAGE_KEY];
            if (!string.IsNullOrEmpty(shipJumpRangeStoredValue))
            {
                ShipJumpRange = float.Parse(shipJumpRangeStoredValue);
            }

            var extraDistanceStoredValue = Storage[EXTRA_DISTANCE_STORAGE_KEY];
            if (!string.IsNullOrEmpty(extraDistanceStoredValue))
            {
                AcceptableExtraDistance = float.Parse(extraDistanceStoredValue);
            }

            NotifyStateChanged();
        }
    }
}
