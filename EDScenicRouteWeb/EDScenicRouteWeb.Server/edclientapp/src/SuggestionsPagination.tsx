import {Component} from "react";
import * as React from "react";

export interface Props {
    PageNumber: number;
    ScenicSuggestionPages: number;
    PageNumberChanged: OnPageChangeFunc; 
}

interface OnPageChangeFunc {
    (page: number) : void;
}

interface State {

}

export class SuggestionsPagination extends Component<Props, State>{
    render(){
        if (this.props.ScenicSuggestionPages > 0) {
            return (
                <div className="SuggestionsPagination row justify-content-center">
                    <ul className="pagination flex-wrap">
                        <li className={"page-item " + (this.props.PageNumber > 0 ? "" : "disabled")}>
                            <button className="page-link" onClick={e => {
                                this.props.PageNumberChanged(this.props.PageNumber - 1);
                                e.preventDefault();
                            }}>&laquo;</button>
                        </li>
                        {Array.from(Array(this.props.ScenicSuggestionPages).keys()).map((x, i) =>

                            <li className={"page-item" + (this.props.PageNumber === i ? " active" : "")} key={i}>
                                <button className="page-link" onClick={e => {
                                    this.props.PageNumberChanged(i);
                                    e.preventDefault();
                                }}>{i + 1}</button>
                            </li>
                        )}
                        <li className={"page-item " + ((this.props.PageNumber < (this.props.ScenicSuggestionPages - 1)) ? "" : "disabled")}>
                            <button className="page-link" onClick={e => {
                                this.props.PageNumberChanged(this.props.PageNumber + 1);
                                e.preventDefault();
                            }}>&raquo;</button>
                        </li>
                    </ul>
                </div>
            );
        } else {
            return <div className="SuggestionsPagination" />
        }
    }
}

export default SuggestionsPagination