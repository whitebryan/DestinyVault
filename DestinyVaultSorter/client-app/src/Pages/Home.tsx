import React from 'react';
import logo from '../logo.svg';
import '../App.css';
import { Link } from 'react-router-dom';

function HomePage() {
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <p>
            <Link to="/vault">Go to vault page.</Link>
          </p>  
        </header>
      </div>
    );
  }

export default HomePage;