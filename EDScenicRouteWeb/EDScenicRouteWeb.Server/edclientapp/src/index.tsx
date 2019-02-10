import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import 'bootswatch/dist/darkly/bootstrap.min.css';
import './site.css';
import 'reactstrap';
import {BrowserRouter} from "react-router-dom";

ReactDOM.render(
    <BrowserRouter>
        <App />
    </BrowserRouter>,
    document.getElementById('root')
);