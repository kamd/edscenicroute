import {GalacticPOI} from './GalacticPOI';

export class ScenicSuggestionViewModel {

    constructor(ExtraDistance: number, PercentageAlongRoute: number, POI: GalacticPOI) {
        this.ExtraDistance = ExtraDistance;
        this.PercentageAlongRoute = PercentageAlongRoute;
        this.POI = POI;
    }

    ExtraDistance : number;
    PercentageAlongRoute: number;
    POI: GalacticPOI;
}