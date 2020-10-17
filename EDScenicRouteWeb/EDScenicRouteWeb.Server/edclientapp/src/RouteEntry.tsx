import {Component} from "react";
import * as React from "react";
import TypeAheadTextBox from "./TypeAheadTextBox";
import {Button, Card, CardBody, FormGroup, Input, Label} from "reactstrap";

interface OnSearchClickFunc {
    (From:string, To:string, JumpRange:number, AcceptableExtraJumps:number) : void;
}

interface Props {
    OnSearchClick: OnSearchClickFunc;
    AutocompleteApiUrl: string;
    JumpRange: number;
    OnJumpRangeChanged: (range: number) => void;
    AcceptableExtraJumps: number;
    OnExtraJumpsChanged: (jumps: number) => void;
}

interface State {
    From: string;
    To: string;
    DefaultFrom: string;
    DefaultTo: string;
}

export class RouteEntry extends Component<Props, State>{

    constructor(props: Readonly<Props>) {
        super(props);
        this.state = {
            DefaultFrom: "Sol",
            DefaultTo: "Colonia",
            From: "",
            To: ""
        };
    }
    
    OnSearch = () => this.props.OnSearchClick(
        this.state.From !== "" ? this.state.From : this.state.DefaultFrom, 
        this.state.To !== "" ? this.state.To : this.state.DefaultTo,
        this.props.JumpRange,
        this.props.AcceptableExtraJumps);

    HandleJumpRangeChange = (event: React.FormEvent<HTMLInputElement>) => {
        this.props.OnJumpRangeChanged(event.currentTarget.valueAsNumber);
    };
    
    HandleAcceptableJumps = (event: React.FormEvent<HTMLInputElement>) => {
        this.props.OnExtraJumpsChanged(event.currentTarget.valueAsNumber);
    };

    render(){
        return (
            <div className="RouteEntry">
            <p>Enter trip details below.</p>
            <Card className="card text-white border-secondary mb-3">
                <CardBody className="card-body">
                    <div className="row">
                        <div className="col-sm-6">
                            <FormGroup>
                                <fieldset>
                                    <Label for="fromTextBox">From:</Label>
                                    <TypeAheadTextBox
                                        Placeholder={this.state.DefaultFrom}
                                        Id="fromTextBox"
                                        Value={this.state.From}
                                        OnInputChange={v => this.setState({From: v})} ApiUrl={this.props.AutocompleteApiUrl}/>
                                </fieldset>
                            </FormGroup>
                            <div className="form-group">
                                <fieldset>
                                    <Label for="toTextBox">To:</Label>
                                    <TypeAheadTextBox
                                        Placeholder={this.state.DefaultTo}
                                        Id="toTextBox"
                                        Value={this.state.To}
                                        OnInputChange={v => this.setState({To: v})} ApiUrl={this.props.AutocompleteApiUrl}/>
                                </fieldset>
                            </div>
                        </div>
                        <div className="col-sm-6">
                            <FormGroup>
                                <Label>Jump Range of Ship: </Label><Input value={this.props.JumpRange} onChange={this.HandleJumpRangeChange} type="number" min="10" max="80"/>
                            </FormGroup>
                            <FormGroup>
                                <Label>Acceptable Extra Jumps:</Label>
                                <Input value={this.props.AcceptableExtraJumps} onChange={this.HandleAcceptableJumps}
                                       type="number" min="1" max="150"/>
                            </FormGroup>
                            <Button color="primary" onClick={this.OnSearch}>Search for POIs to visit</Button>
                        </div>
                    </div>
                </CardBody>
            </Card>
            </div>
        );
    }
    
    
}

export default RouteEntry