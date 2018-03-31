using System;
using System.Collections.Generic;
using System.Text;

namespace EDScenicRouteCoreModels
{
    [Serializable]
    public class ScenicSuggestionResults
    {
        public List<ScenicSuggestion> Suggestions { get; set; }
        public float StraightLineDistance { get; set; }
    }
}
