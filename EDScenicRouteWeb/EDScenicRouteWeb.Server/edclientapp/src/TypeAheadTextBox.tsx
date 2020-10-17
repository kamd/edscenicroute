import {Component} from "react";
import * as React from "react";
import {Input} from "reactstrap";

export interface Props {
    Value: string;
    Placeholder: string;
    OnInputChange: OnInputChangeFunc;
    Id: string;
    ApiUrl: string;
}

interface OnInputChangeFunc {
    (Value:string) : void;
}

interface State {
    Suggestions: string[];
}

export class TypeAheadTextBox extends Component<Props, State>{

    constructor(props: Readonly<Props>) {
        super(props);
        this.state = {Suggestions: []};
    }

    ItemClicked = (item: string) => {
        this.props.OnInputChange(item);
        this.setState({Suggestions: []});
    };
    
    TextChanged = async (e: React.FormEvent<HTMLInputElement>) => {
        const input = e.currentTarget.value;
        this.props.OnInputChange(input);
        if (input.length >= 4){
           const response = await fetch(`${this.props.ApiUrl}${input}`, 
               {
                   headers: {
                       'Accept': 'application/json',
                       'Content-Type': 'application/json'
                   }
               });
           if (response.ok){
               const suggestions = await response.json();
               this.setState({Suggestions: suggestions});
           } else {
               this.setState({Suggestions: []});
           }
        }
    };
    
    render(){
        return (
            <div className="dropdown" onBlur={() => this.setState({Suggestions: []})}>
                <Input id={this.props.Id} type="text" placeholder={this.props.Placeholder}
                       value={this.props.Value} 
                       onChange={this.TextChanged}
                />
                <div className={"dropdown-menu" + (this.state.Suggestions.length > 0 ? " show" : undefined)}>
                    {this.state.Suggestions.map((x, i) =>
                        <button key={i} className="dropdown-item" onMouseDown={() => this.ItemClicked(x)}>
                            {x}
                        </button>
                        
                    )}
                </div>
            </div>
        );  
    }
}

export default TypeAheadTextBox