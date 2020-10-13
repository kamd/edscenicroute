import React, {Component} from 'react';

import {ScenicSuggestionViewModel} from "./models/ScenicSuggestionViewModel";
import {RouteDetails} from "./RouteDetails";
import {RouteEntry} from "./RouteEntry"
import {ScenicResults} from "./ScenicResults";
import {ScenicSuggestionsResponse} from "./models/ScenicSuggestionsResponse";

export interface Props {

}

interface State {
    Suggestions: ScenicSuggestionViewModel[];
    CurrentlySearching: boolean;
    ErrorMessage?: string;
    SearchCompleted: boolean;
    StraightLineDistanceOfTrip: number;
    RouteDetails: RouteDetails;
    ShipJumpRange: number;
    AcceptableExtraJumps: number;
    AcceptableExtraDistance: number;
    ResultsPageNumber: number;
}

export class Home extends Component<Props, State> {
    
    SHIP_JUMP_RANGE_KEY = "ShipJumpRange";
    ACCEPTABLE_EXTRA_DISTANCE_KEY = "AcceptableExtraDistance";

    constructor(props: Readonly<Props>) {
        super(props);

        // Check local storage
        const storedJumpRange = Number(localStorage[this.SHIP_JUMP_RANGE_KEY]);
        const storedExtraDistance = Number(localStorage[this.ACCEPTABLE_EXTRA_DISTANCE_KEY]);
        const jumpRange = isNaN(storedJumpRange) ? 30 : storedJumpRange;
        const extraDistance = isNaN(storedExtraDistance) ? 150 : storedExtraDistance;
        
        this.state = {
            Suggestions: [],
            CurrentlySearching: false,
            ErrorMessage: undefined,
            RouteDetails: {FromSystemName: "", ToSystemName: "", AcceptableExtraDistance: extraDistance},
            SearchCompleted: false,
            StraightLineDistanceOfTrip: 0,
            AcceptableExtraDistance: extraDistance,
            AcceptableExtraJumps: extraDistance / jumpRange,
            ShipJumpRange: jumpRange,
            ResultsPageNumber: 0
        } as State;
    }
    
    OnSearchClick = async (from: string, to: string) => {
        const acceptableExtraDistance = this.state.ShipJumpRange * this.state.AcceptableExtraJumps;
        localStorage[this.ACCEPTABLE_EXTRA_DISTANCE_KEY] = acceptableExtraDistance;
        this.setState({AcceptableExtraDistance: acceptableExtraDistance, CurrentlySearching: true, SearchCompleted: false});
        
        const response = await fetch('/api/scenicsuggestions',
            {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({FromSystemName: from, ToSystemName: to, AcceptableExtraDistance: acceptableExtraDistance})
            });

        const content: ScenicSuggestionsResponse = await response.json();
        if (!response.ok){
            this.setState({ErrorMessage: "Error: " + content, StraightLineDistanceOfTrip: 0, Suggestions: [], CurrentlySearching: false});
            return;
        } 
        
        this.setState({
            StraightLineDistanceOfTrip: content.straightLineDistance,
            Suggestions: content.suggestions,
            CurrentlySearching: false,
            ErrorMessage: undefined,
            ResultsPageNumber: 0
        });
    };
    
    OnJumpRangeChanged = (range: number) => {
        this.setState({ShipJumpRange: range});
        localStorage[this.SHIP_JUMP_RANGE_KEY] = range;
    };

    OnExtraJumpsChanged = (jumps: number) => {
        this.setState({AcceptableExtraJumps: jumps});
    };
    
    render() {
        return (
            <div className="Home">
                <h1>Scenic Route Finder</h1>
                <div id="scenic-routes-area">
                    <RouteEntry
                        OnSearchClick={this.OnSearchClick}
                        AutocompleteApiUrl={"/api/poitypeahead/"}
                        JumpRange={this.state.ShipJumpRange}
                        AcceptableExtraJumps={this.state.AcceptableExtraJumps}
                        OnJumpRangeChanged={this.OnJumpRangeChanged}
                        OnExtraJumpsChanged={this.OnExtraJumpsChanged}
                    />
                </div>
                <div id="suggestions-area">
                    <ScenicResults
                        CurrentlySearching={this.state.CurrentlySearching}
                        StraightLineDistance={this.state.StraightLineDistanceOfTrip}
                        ScenicSuggestions = {this.state.Suggestions}
                        ErrorMessage = {this.state.ErrorMessage}
                        ShipJumpRange = {this.state.ShipJumpRange}
                        MaxExtraDistance = {this.state.AcceptableExtraDistance}
                        SearchCompleted = {this.state.SearchCompleted}
                        PageNumber = {this.state.ResultsPageNumber}
                        OnPageChanged={p => this.setState({ResultsPageNumber: p})}
                    />
                </div>
            </div>
        );
    }
}

export default Home;
