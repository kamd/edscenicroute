import {ScenicSuggestionViewModel} from "./ScenicSuggestionViewModel";

export class ScenicSuggestionsResponse {
    constructor(
        public suggestions: ScenicSuggestionViewModel[],
        public straightLineDistance: number) 
    {
    }
}