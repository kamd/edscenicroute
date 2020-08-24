import {Component} from "react";
import * as React from "react";
import {ScenicSuggestionViewModel} from "./models/ScenicSuggestionViewModel";
import {GalacticPOIType} from "./models/GalacticPOI";
import {Card, CardBody, Progress} from "reactstrap";
import LocationInfo from "./LocationInfo";
import PlanetaryLocationAddon from "./PlanetaryLocationAddon";

export interface Props {
    Item: ScenicSuggestionViewModel;
    Type : GalacticPOIType;
    ShipJumpRange: number;
    MaxExtraDistance: number;
}

interface State {

}

export class ScenicSuggestionView extends Component<Props, State>{
    
    constructor(props: Readonly<Props>) {
        super(props);
        
    }

    ProgressBarColour = () => {
        const distancePercent = this.DistancePercent();
        if (distancePercent > 80)
        {
            return "danger";
        }
        if (distancePercent > 50)
        {
            return "warning";
        }
        return "info";
    };
    
    DistancePercent = () => Math.round(100 * (this.props.Item.ExtraDistance / this.props.MaxExtraDistance));
    
    SelectBox = (e: React.MouseEvent) => {
        const node = document.getElementById(e.currentTarget.id);
        if (node != null) {
            window.getSelection()?.selectAllChildren(node);
        }
    };

    ExtraJumps = (distance: number) => {
        const jumps = Math.ceil(distance / this.props.ShipJumpRange);
        return jumps > 1 ? `${jumps} extra jumps` : "1 extra jump";
    };
    
    render(){
        return (
            
            <Card className="border-primary mb-3">
                <CardBody className="ssview">
                    <div className="row align-items-center">
                        <div className="col-3 col-sm-1 poi-icon-column">
                            <span className={this.props.Type.DisplayClass + " poi-icon"}></span>
                        </div>
                        <div className="col-9 col-sm-4">
                            <span className="poi-name">{this.props.Item.POI.Name}</span>
                            <br />
                            <span><i>{this.props.Type.Name}</i></span>
                        </div>
                        <div className="col-12 col-sm">
                            <div className="row align-items-center justify-content-center">
                                <div className="col ssview-col">
                                    <div className="ssview-info">
                                        <span>{this.ExtraJumps(this.props.Item.ExtraDistance)}, <span className="text-info">{this.props.Item.ExtraDistance.toFixed(2)} Ly</span></span>
                                    </div>
                                </div>
                            </div>
                            <div className="row align-items-center justify-content-center">
                                <div className="col ssview-col">
                                    <Progress className="poi-jumps-bar" color={this.ProgressBarColour()} value={this.DistancePercent()} />
                                </div>
                            </div>
            
                            <div className="row align-items-center justify-content-center">
                               <div className="col-xl-4 col-lg-5 col-md-6 col-sm-7 ssview-col ssview-faralong-left">
                                    <div className="ssview-info">
                                        <span>{this.props.Item.PercentageAlongRoute.toFixed(0)}% <i className="fas fa-angle-double-right"></i></span>
                                    </div>
                                </div>
                                <div className="col-xl-8 col-lg-7 col-md-6 col-sm-5 ssview-col ssview-faralong-right">
                                    <Progress className="poi-jumps-bar" barClassName="progress-bar-striped" value={this.props.Item.PercentageAlongRoute}
                                              data-toggle="tooltip" title={this.props.Item.PercentageAlongRoute.toFixed(0) + "% along the route"} />
                                </div>
                            </div>
                        </div>
                        <div className="col">
                            <LocationInfo Item={this.props.Item} Type={this.props.Type} />
                        </div>
                        <div className="col">
                            <div className="row">
                                <div className="col">
                                    <a className="btn btn-info" role="button" href={this.props.Item.POI.GalMapUrl}>EDSM Info</a>
                                </div>
                            {this.props?.Item?.POI?.Body != null ?
                                <div className="col">
                                    <PlanetaryLocationAddon Item={this.props.Item} Type={this.props.Type} />
                                </div>
                                : null}
                            </div>
                        </div>
                    </div>
                </CardBody>
            </Card>

        );
    }
}

export default ScenicSuggestionView