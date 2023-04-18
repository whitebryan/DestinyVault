import React from 'react';
import logo from './logo.svg';
import './App.css';
import Button from './components/RoundButton'

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>  
        <Button
          border='none'
          color='pink'
          height='150px'
          onClick={() => populateWeapons()}
          radius='50%'
          width='150px'
          children = "Weapons"
          />
      </header>
    </div>
  );
}

async function populateWeapons() {
    const resposne = await fetch("api/Weapons");
    const data = await resposne.json();
    return data;
}

export default App;
