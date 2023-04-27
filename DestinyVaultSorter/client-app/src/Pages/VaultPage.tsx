import React, { useState} from 'react';
import {useTable} from "react-table";
import logo from '../logo.svg';
import '../App.css';
import { Link } from 'react-router-dom';
import Button from '../components/RoundButton';
import WeaponTable from '../components/WeaponTable';
import DropDown from '../components/DropDownMenu';

let weaponTypes = [
    "Any",
    "Bow",
    "Shotgun",
    "Sniper",
];

let weaponElements = [
    "Any",
    "Kinetic",
    "Arc",
    "Solar",
    "Void",
    "Stasis",
    "Strand",
];

let weaponSlots = [
    "Any",
    "Kinetic",
    "Elemental",
    "Heavy",
];

function VaultPage() {

    const [movePage, setPage] = useState(0);
    const [type, setType] = useState("Any");
    const [element, setElement] = useState("Any");
    const [slot, setSlot] = useState("Any");
    const [level, setLevel] = useState(0);

    function changePage(direction: number){
        setPage(direction);
    }

    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <p>
            <Link to="/">Go Home.</Link>
          </p>
          <Button border='none'
            color='pink'
            height='50px'
            onClick={() => changePage(-1)}
            radius='50%'
            width='50px'
            children = "Back"
            />
                      <Button
            border='none'
            color='pink'
            height='50px'
            onClick={() => changePage(1)}
            radius='50%'
            width='50px'
            children = "Forward"
            />
            <DropDown options={weaponTypes} default={0} valueChange={setType} label='Weapon Type'/>
            <DropDown options={weaponElements} default={0} valueChange={setElement} label='Weapon Element'/>
            <DropDown options={weaponSlots} default={0} valueChange={setSlot} label='Weapon Slot'/>
            <WeaponTable page={movePage} setPage={setPage} type={type} element={element} level={level} slot={slot}/>
        </header>
      </div>
    );
  }

  export default VaultPage;