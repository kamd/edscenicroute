import {GalacticPOI} from './GalacticPOI';

export class ScenicSuggestionViewModel {
    constructor(
        public extraDistance: number,
        public percentageAlongRoute: number, 
        public poi: GalacticPOI) {
    }
}