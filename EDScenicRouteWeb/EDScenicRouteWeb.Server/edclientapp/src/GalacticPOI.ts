export class GalacticPOI {

    constructor(Id: number, Type: GalacticPOITypeEnum, Name: string, GalMapSearch: string, GalMapUrl: string, Coordinates: Vector3, DistanceFromSol: number) {
        this.Id = Id;
        this.Type = Type;
        this.Name = Name;
        this.GalMapSearch = GalMapSearch;
        this.GalMapUrl = GalMapUrl;
        this.Coordinates = Coordinates;
        this.DistanceFromSol = DistanceFromSol;
    }

    Id: number;
    Type: GalacticPOITypeEnum;
    Name: string;
    GalMapSearch: string;
    GalMapUrl: string;
    Coordinates: Vector3;
    DistanceFromSol: number;
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
    geyserPOI
}

export class GalacticPOIType {
    Type: GalacticPOITypeEnum;
    Name: string;
    DisplayClass: string;

    constructor(Type: GalacticPOITypeEnum, Name: string, DisplayClass: string) {
        this.Type = Type;
        this.Name = Name;
        this.DisplayClass = DisplayClass;
    }
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
            {Type: GalacticPOITypeEnum.geyserPOI, Name: "Geyser POI", DisplayClass: "fas fa-cloud-upload-alt"}
        ];
        return alltypes;
    };
    
    NullType = () => {
        return {Name: "Unknown", Type: GalacticPOITypeEnum.mysteryPOI, DisplayClass: ""}
    };
}

export class Vector3{

    constructor(X: number, Y: number, Z: number) {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    X: number;
    Y: number;
    Z: number;
}
