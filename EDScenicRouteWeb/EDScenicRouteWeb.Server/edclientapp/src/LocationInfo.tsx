import {Component} from "react";
import * as React from "react";
import {ScenicSuggestionViewModel} from "./models/ScenicSuggestionViewModel";
import {GalacticPOIType} from "./models/GalacticPOI";

export interface Props {
    Item: ScenicSuggestionViewModel;
    Type : GalacticPOIType;
}

interface State {

}

export class LocationInfo extends Component<Props, State>{


    constructor(props: Readonly<Props>) {
        super(props);

    }

    SelectBox = (e: React.MouseEvent) => {
        const node = document.getElementById(e.currentTarget.id);
        if (node != null) {
            window.getSelection()?.selectAllChildren(node);
        }
    };

    render(){
        return (
            <div className="ssview-system">
                System: <br/><code id={"item-" + this.props.Item.poi.id} onClick={this.SelectBox}>{this.props.Item.poi.galMapSearch}</code>
            </div>
        );
    }
}

export default LocationInfo