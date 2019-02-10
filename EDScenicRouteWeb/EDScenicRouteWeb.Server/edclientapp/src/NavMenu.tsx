import {Component} from "react";
import * as React from "react";
import {Nav, NavItem} from "reactstrap";
import {NavLink} from "react-router-dom";

interface Props {

}

interface State {
    Collapse: boolean;
}

export class NavMenu extends Component<Props, State>{

    constructor(props: Readonly<Props>) {
        super(props);
        this.state = {
            Collapse: false
        }
    }

    ToggleNavMenu = (e : React.MouseEvent) => this.setState({Collapse: !this.state.Collapse});
    
    render(){
        return (
            <div className="NavMenu sidebar">
                <div className="top-row pl-4 navbar navbar-dark">
                    <a className="navbar-brand" href="/">ED: Scenic Route Finder</a>
                    <button className="navbar-toggler" onClick={this.ToggleNavMenu}>
                        <span className="navbar-toggler-icon"></span>
                    </button>
                </div>

                <div className={this.state.Collapse ? "collapse" : undefined}>
                    <Nav className="nav flex-column">
                        <NavItem className="px-3">
                            <NavLink className="nav-link" exact={true} to="/" activeClassName="active">
                                <span className="navlink-icon fas fa-search" aria-hidden="true"></span>Route Finder
                            </NavLink>
                        </NavItem>
                        <NavItem className="px-3">
                            <NavLink className="nav-link" to="/about" activeClassName="active">
                                <span className="navlink-icon fas fa-question-circle" aria-hidden="true"></span>About
                            </NavLink>
                        </NavItem>
                    </Nav>
                </div>
            </div>
        );
    }
}

export default NavMenu