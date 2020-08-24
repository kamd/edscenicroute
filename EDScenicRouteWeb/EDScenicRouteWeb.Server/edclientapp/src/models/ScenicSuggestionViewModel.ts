import {GalacticPOI} from './GalacticPOI';

export class ScenicSuggestionViewModel {
    constructor(
        public ExtraDistance: number,
        public PercentageAlongRoute: number, 
        public POI: GalacticPOI) {
    }
}