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
                <Button id={"planetary-popover-" + this.props.Item.poi.id} type="button">
                    <i className="fas fa-globe"></i>
                </Button>
                <Popover placement="bottom" isOpen={this.state.popoverVisible} target={"planetary-popover-" + this.props.Item.poi.id} toggle={this.Toggle}>
                    <PopoverHeader>Planetary POI</PopoverHeader>
                    <PopoverBody><div>Body: <strong>{this.props.Item.poi.body}</strong><br/>
                        Lat: <strong>{this.props.Item.poi.latitude}</strong> <br/>
                        Long: <strong>{this.props.Item.poi.longitude}</strong></div></PopoverBody>
                </Popover>
            </div>
        );
    }
}

export default PlanetaryLocationAddon