import React, { Component } from 'react';
import {NavMenu} from "./NavMenu";
import {Home} from "./Home";
import {Route, Switch} from "react-router";
import About from "./About";

export interface Props {
    
}

interface State {

}

class App extends Component<Props, State> {

    constructor(props: Readonly<Props>) {
        super(props);
    }

    render() {
    return (
          <div className="App">
            <NavMenu />
            <div className="main">
              <div className="content px-4">
                  <Switch>
                      <Route exact path="/" component={Home} />
                      <Route path="/about" component={About} />
                  </Switch>
              </div>
            </div>
          </div>
      );
  }
}

export default App;
