import {Component} from "react";
import * as React from "react";
import {ScenicSuggestionViewModel} from "./models/ScenicSuggestionViewModel";
import {GalacticPOIType} from "./models/GalacticPOI";
import {Button, Popover, PopoverBody, PopoverHeader} from "reactstrap";

export interface Props {
    Item: ScenicSuggestionViewModel;
    Type : GalacticPOIType;
}

interface State {
    popoverVisible: boolean
}

export class PlanetaryLocationAddon extends Component<Props, State>{
    
    constructor(props: Readonly<Props>) {
        super(props);

        this.state = {popoverVisible: false};
    }
    
    Toggle = () => {
        this.setState((prevState, props) => ({popoverVisible: !prevState.popoverVisible}));
    }

    render() {
        return (
            <div>
                <Button id={"planetary-popover-" + this.props.Item.POI.Id} type="button">
                    <i className="fas fa-globe"></i>
                </Button>
                <Popover placement="bottom" isOpen={this.state.popoverVisible} target={"planetary-popover-" + this.props.Item.POI.Id} toggle={this.Toggle}>
                    <PopoverHeader>Planetary POI</PopoverHeader>
                    <PopoverBody><div>Body: <strong>{this.props.Item.POI.Body}</strong><br/>
                        Lat: <strong>{this.props.Item.POI.Latitude}</strong> <br/>
                        Long: <strong>{this.props.Item.POI.Longitude}</strong></div></PopoverBody>
                </Popover>
            </div>
        );
    }
}

export default PlanetaryLocationAddon