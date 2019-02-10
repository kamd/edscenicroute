export class RouteDetails {
    FromSystemName: string;
    ToSystemName: string;
    AcceptableExtraDistance: number;


    constructor(FromSystemName: string, ToSystemName: string, AcceptableExtraDistance: number) {
        this.FromSystemName = FromSystemName;
        this.ToSystemName = ToSystemName;
        this.AcceptableExtraDistance = AcceptableExtraDistance;
    }
}