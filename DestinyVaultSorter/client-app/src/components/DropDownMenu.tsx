import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import React, { CSSProperties } from 'react';

interface dropProps{
    options: string[],
    default: number,
    label: string,
    valueChange: (newVal: string) => void,
}

function DropDown(props: dropProps){
    const [selectedValue, setValue] = React.useState(props.options[props.default]);

    function onValChange(event: SelectChangeEvent ){
        props.valueChange(event.target.value as string);
        setValue(event.target.value as string);
    }

    return(
        <Box sx={{minWidth: 120, backgroundColor: '#1976d2'}}>
            <FormControl fullWidth variant='filled' sx={{color: 'white'}}>
                <InputLabel sx={{color: 'white'}}>{props.label}</InputLabel>
                <Select labelId={props.label} value={selectedValue} label={props.label} onChange={onValChange} sx={{backgroundColor: '#1976d2', color: 'white'}}>
                    {
                        props.options.map((curOption) => {
                            return <MenuItem value={curOption}>{curOption}</MenuItem>;
                        })
                    }
                </Select>
            </FormControl>
        </Box>
    );
}

export default DropDown;