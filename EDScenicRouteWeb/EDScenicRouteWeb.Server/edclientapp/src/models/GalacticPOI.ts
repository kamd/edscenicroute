export class GalacticPOI {

    constructor(
        public id: number,
        public type: GalacticPOITypeEnum,
        public name: string,
        public galMapSearch: string,
        public galMapUrl: string,
        public x: number,
        public y: number,
        public z: number,
        public body: string,
        public latitude: number,
        public longitude: number,
        public distanceFromSol: number) { }
}

export enum GalacticPOITypeEnum
{
    planetaryNebula,
    nebula,
    blackHole,
    historicalLocation,
    stellarRemnant,
    planetFeatures,
    minorPOI,
    regional,
    pulsar,
    starCluster,
    surfacePOI,
    deepSpaceOutpost,
    mysteryPOI,
    organicPOI,
    geyserPOI,
    alienCivStructure = 101,
    organicStructure,
    geologyAnomalies
}

export class GalacticPOIType {
    constructor(
        public Type: GalacticPOITypeEnum,
        public Name: string,
        public DisplayClass: string) { }
}

export class GalacticPOIHelper {
    AllGalacticPOITypes = () => {
        const alltypes:GalacticPOIType[] = [
            {Type: GalacticPOITypeEnum.planetaryNebula, Name: "Planetary Nebula", DisplayClass: "fas fa-sun"},
            {Type: GalacticPOITypeEnum.nebula, Name: "Nebula", DisplayClass: "fas fa-fire"},
            {Type: GalacticPOITypeEnum.blackHole, Name: "Black Hole", DisplayClass: "fas fa-genderless"},
            {Type: GalacticPOITypeEnum.historicalLocation, Name: "Historical Location", DisplayClass: "fas fa-chess-rook"},
            {Type: GalacticPOITypeEnum.stellarRemnant, Name: "Stellar Remnant", DisplayClass: "fas fa-compress"},
            {Type: GalacticPOITypeEnum.planetFeatures, Name: "Planet Features", DisplayClass: "fas fa-globe-americas"},
            {Type: GalacticPOITypeEnum.minorPOI, Name: "Minor POI", DisplayClass: "fas fa-map-pin"},
            {Type: GalacticPOITypeEnum.regional, Name: "Regional", DisplayClass: "fas fa-arrows-alt"},
            {Type: GalacticPOITypeEnum.pulsar, Name: "Pulsar", DisplayClass: "fas fa-compact-disc"},
            {Type: GalacticPOITypeEnum.starCluster, Name: "Star Cluster", DisplayClass: "fas fa-ellipsis-h"},
            {Type: GalacticPOITypeEnum.surfacePOI, Name: "Surface POI", DisplayClass: "fas fa-download"},
            {Type: GalacticPOITypeEnum.deepSpaceOutpost, Name: "Deep Space Outpost", DisplayClass: "fas fa-building"},
            {Type: GalacticPOITypeEnum.mysteryPOI, Name: "Mystery POI", DisplayClass: "fas fa-question-circle"},
            {Type: GalacticPOITypeEnum.organicPOI, Name: "Organic POI", DisplayClass: "fas fa-tree"},
            {Type: GalacticPOITypeEnum.geyserPOI, Name: "Geyser POI", DisplayClass: "fas fa-cloud-upload-alt"},
            {Type: GalacticPOITypeEnum.alienCivStructure, Name: "Alien Civilisation Structure", DisplayClass: "fas fa-robot"},
            {Type: GalacticPOITypeEnum.organicStructure, Name: "Organic Codex Entry", DisplayClass: "fas fa-seedling"},
            {Type: GalacticPOITypeEnum.geologyAnomalies, Name: "Geological/Anomalous Codex Entry", DisplayClass: "fas fa-mountain"}
        ];
        return alltypes;
    };
    
    NullType = () => {
        return {Name: "Unknown", Type: GalacticPOITypeEnum.mysteryPOI, DisplayClass: ""}
    };
}
