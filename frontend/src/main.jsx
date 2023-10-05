import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import {Provider} from 'react-redux'; 
import store from './store.jsx';

import {positions, transitions, Provider as AlertProvider} from 'react-alert'; 
import  AlertTemplate from 'react-alert';

const options = {
  timeout: 500, 
  offset:'30px',
  position: positions.BOTTOM_CENTER,
  transition: transitions.SCALE
}; 


ReactDOM.createRoot(document.getElementById('root')).render(
  <Provider store={store}>
    <AlertProvider template={AlertTemplate}{...options}>
    <App />

    </AlertProvider>
  </Provider>,
)
