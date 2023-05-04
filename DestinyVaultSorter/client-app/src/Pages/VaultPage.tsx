import React, { useState} from 'react';
import '../App.css';
import Button from '@mui/material/Button'
import Stack from '@mui/material/Stack';
import WeaponTable from '../components/WeaponTable';
import DropDown from '../components/DropDownMenu';
import Slider from '@mui/material/Slider';
import Input from '@mui/material/Input';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';

let weaponTypes = [
    "Any",
    "Hand Cannon",
    "Auto Rifle",
    "Submachine Gun",
    "Combat Bow",
    "Pulse Rifle",
    "Scout Rifle",
    "Sidearm",
    "Shotgun",
    "Trace Rifle",
    "Grenade Launcher",
    "Sniper Rifle",
    "Fusion Rifle",
    "Glaive",
    "Sword",
    "Machine Gun",
    "Linear Fusion Rifle",
    "Rocket Launcher",
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
    const [curPage, setCurPage] = useState(0);
    const [maxPages, setMaxPages] = useState(0);
    const [type, setType] = useState("Any");
    const [element, setElement] = useState("Any");
    const [slot, setSlot] = useState("Any");
    const [level, setLevel] = useState(1600);

    function changePage(direction: number){
      setPage(direction);
    }

    function sliderValueChange(event: Event, newValue: number | number[]){
      setLevel(newValue as number);
    }

    function textValueChange(event: React.ChangeEvent<HTMLInputElement>){
      setLevel(Number(event.target.value));
    }

    function handleBlur(){
      if(level < 1600){
        setLevel(1600);
      }
      else if(level > 1810){
        setLevel(1810);
      }
    }

    return (
      <div className="App">
        <header className="App-header">
          <Stack direction="row" spacing={2} alignItems='center'>
            <DropDown options={weaponTypes} default={0} valueChange={setType} label='Weapon Type'/>
            <DropDown options={weaponElements} default={0} valueChange={setElement} label='Weapon Element'/>
            <DropDown options={weaponSlots} default={0} valueChange={setSlot} label='Weapon Slot'/>
            <Typography gutterBottom>Weapon Level</Typography>
            <Slider value={level} min={1600} max={1810} onChange={sliderValueChange} aria-labelledby="input-slider"/>
            <Input sx={{color: 'white'}} fullWidth value={level} size='medium' onChange={textValueChange} onBlur={handleBlur} inputProps={{step: 10, min: 1600, max: 1810, type: 'number', 'aria-labelledby': "input-slider",}}/>
          </Stack>
          <WeaponTable page={movePage} setPage={setPage} type={type} element={element} level={level} slot={slot} setCurPage={setCurPage} setMaxPages={setMaxPages}/>
          <Stack direction="row" spacing={2}>
            <Button variant='contained' onClick={() => changePage(-1)} disabled={curPage <= 0}>
              Previous Page
            </Button>
            <Button variant='contained' onClick={() => changePage(1)} disabled={curPage >= maxPages - 5}>
              Next Page
            </Button>
          </Stack>
        </header>
      </div>
    );
  }

  export default VaultPage;