import React from 'react';
import ReactDOM from 'react-dom/client';
import './app/layout/styles.css';
import App from './app/layout/App';
import reportWebVitals from './reportWebVitals';
import {createBrowserHistory} from "history";
import { Router } from 'react-router-dom';
import { StoreProvider } from './app/context/StorreContext';


export const history = createBrowserHistory();

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
    <Router history={history}>
      <StoreProvider>
      <App />
      </StoreProvider>
    </Router>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

