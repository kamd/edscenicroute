import {Component} from "react";
import React from "react";
import {ScenicSuggestionViewModel} from "./models/ScenicSuggestionViewModel";
import {GalacticPOIHelper, GalacticPOIType, GalacticPOITypeEnum} from "./models/GalacticPOI";
import {SuggestionsPagination} from "./SuggestionsPagination";
import {ScenicSuggestionView} from "./ScenicSuggestionView";
import {Alert, Button, ButtonGroup, InputGroupAddon} from "reactstrap";

interface Props {
    CurrentlySearching: boolean;
    StraightLineDistance:number;
    ScenicSuggestions: ScenicSuggestionViewModel[];
    ErrorMessage ?: string;
    ShipJumpRange:number;
    MaxExtraDistance :number;
    SearchCompleted : boolean;
    PageNumber: number;
    OnPageChanged: OnPageChangedFunc;
}

interface OnPageChangedFunc {
    (page: number) : void;
}

class POIFilter {
    Type: GalacticPOIType;
    Show: boolean;

    constructor(Type: GalacticPOIType, Filtered: boolean) {
        this.Type = Type;
        this.Show = Filtered;
    }
}

interface State {
    SortByExtraJumps: boolean;
    POIFilters: POIFilter[];
}

export class ScenicResults extends Component<Props, State> {
    
    constructor(props: Readonly<Props>) {
        super(props);
        const allPOITypes = new GalacticPOIHelper().AllGalacticPOITypes();
        const poiFilters = allPOITypes.map(t => new POIFilter(t, this.ShouldShowByDefault(t.Type)));
        this.state = {
            SortByExtraJumps: true,
            POIFilters: poiFilters
        }
    }
    
    ShouldShowByDefault = (type: GalacticPOITypeEnum) => ![GalacticPOITypeEnum.organicStructure, GalacticPOITypeEnum.geologyAnomalies].includes(type);
    
    SuggestionsPerPage = 25;
    
    FilterAll = (filter: boolean) => {
        this.setState({POIFilters: this.state.POIFilters.map(f => new POIFilter(f.Type, filter))});
        this.props.OnPageChanged(0);
    };
    
    FilterClick = (filter: POIFilter) => {
        let newFilters = this.state.POIFilters.map(f => new POIFilter(f.Type, f.Show));
        const newFilter = newFilters.find(f => f.Type.Type === filter.Type.Type);
        if (newFilter != null) {
            newFilter.Show = !newFilter.Show;    
        }
        this.setState({POIFilters: newFilters});
        this.props.OnPageChanged(0);
    };
    
    FilteredSuggestions = () => {
        return this.props.ScenicSuggestions
            .filter(s => this.state.POIFilters.some(f => f.Type.Type === s.poi.type && f.Show))
            .sort((a, b) => this.state.SortByExtraJumps ? 
                        (a.extraDistance - b.extraDistance) : 
                        (a.percentageAlongRoute - b.percentageAlongRoute));
    };
    
    CurrentPageOfFilteredSuggestions = () => {
        const pageStart = this.props.PageNumber * this.SuggestionsPerPage;
        return this.FilteredSuggestions().slice(pageStart, pageStart + this.SuggestionsPerPage);
    };

    ScenicSuggestionPages = () => Math.ceil((this.FilteredSuggestions().length) / this.SuggestionsPerPage);
    
    SuggestionType = (s : ScenicSuggestionViewModel) => {
        const filter = this.state.POIFilters.find(f => f.Type.Type === s.poi.type);
        if (filter == null){
           return new GalacticPOIHelper().NullType(); 
        } else {
            return filter.Type;
        }
    };

    render() {
        if (this.props.CurrentlySearching) {
            return (
                <div className="ScenicResults">
                    <Alert color="warning">Searching...</Alert>
                </div>
            );
        } else if (this.props.ErrorMessage !== undefined) {
            return (
                <div className="ScenicResults">
                    <Alert color="danger">{this.props.ErrorMessage}</Alert>
                </div>
            );
        } else if (this.props.ScenicSuggestions.length > 0) {
            return (
                <div className="ScenicResults">
                    <div className="row">
                        <div className="col col-sm-6">
                            <Alert color="primary">
                                Straight line distance of your trip: <strong>{this.props.StraightLineDistance.toFixed(2)} Ly</strong>
                            </Alert>
                        </div>
                        <div className="col col-sm-6">
                            <Alert color="info">Consider visiting...</Alert>
                        </div>
                    </div>
        
                    <div className="row justify-content-center filterbar">
                        <ButtonGroup className="flex-wrap">
                            <InputGroupAddon addonType="prepend">Filter POIs:</InputGroupAddon>
                            <Button className="filterbutton" color="primary"
                                    onClick={() => this.FilterAll(false)}>All Off</Button>
                            {this.state.POIFilters.map((x, i) =>
                                <Button key={x.Type.Type} color="primary" active={x.Show} title={x.Type.Name}
                                        onClick={() => this.FilterClick(x)}>
                                    <span className={x.Type.DisplayClass}></span>
                                </Button>
                            )}
                            <Button className="filterbutton" color="primary"
                                    onClick={() => this.FilterAll(true)}>All On</Button>
                        </ButtonGroup>
                    </div>
    
                    <div className="row justify-content-center filterbar">
                        <ButtonGroup>
                            <InputGroupAddon addonType="prepend">Sort by:</InputGroupAddon>
                            <Button color="primary" active={this.state.SortByExtraJumps}
                                    onClick={() => this.setState({SortByExtraJumps: !this.state.SortByExtraJumps})}>
                                 Extra Jumps
                            </Button>
                            <Button color="primary" active={!this.state.SortByExtraJumps}
                                    onClick={() =>this.setState({SortByExtraJumps: !this.state.SortByExtraJumps})}>
                                 Distance Along Route
                            </Button>
                        </ButtonGroup>
                    </div>

                    <SuggestionsPagination
                        PageNumber={this.props.PageNumber}
                        ScenicSuggestionPages={this.ScenicSuggestionPages()}
                        PageNumberChanged={this.props.OnPageChanged}/>
                    {this.CurrentPageOfFilteredSuggestions().map((x, i) =>
                        <ScenicSuggestionView
                            key={x.poi.id}
                            Item={x}
                            ShipJumpRange={this.props.ShipJumpRange}
                            MaxExtraDistance={this.props.MaxExtraDistance}
                            Type={this.SuggestionType(x)}/>
                    )}
                    <SuggestionsPagination
                        PageNumber={this.props.PageNumber}
                        ScenicSuggestionPages={this.ScenicSuggestionPages()}
                        PageNumberChanged={this.props.OnPageChanged}/>
                </div>
            );
        } else if (this.props.SearchCompleted) {
            return (
                <div className="ScenicResults">
                    <Alert color="info">No suggestions.</Alert>
                </div>
            );
        } else {
            return (
                <div className="ScenicResults"></div>
            );
        }
    }
}

export default ScenicResults