using System;
using System.Collections.Generic;

namespace EDScenicRouteWeb.Server.Models
{
    [Serializable]
    public class ScenicSuggestionResults
    {
        public List<ScenicSuggestion> Suggestions { get; set; }
        public float StraightLineDistance { get; set; }
    }
}
