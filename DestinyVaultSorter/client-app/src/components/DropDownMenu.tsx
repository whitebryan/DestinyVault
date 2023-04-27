import { render } from '@testing-library/react';
import React, { useState} from 'react';

interface dropProps{
    options: string[],
    default: number,
    label: string,
    valueChange: (newVal: string) => void,
}

function DropDown(props: dropProps){
    function onValChange(event: { target: { value: string; }; }){
        props.valueChange(event.target.value);
    }

    return(
        <div>
            <label>
                <p>{props.label}</p>
                <select onChange={onValChange} style={{width: 50, height: 50}}>
                    {
                     props.options.map((curOption) => {
                        return <option value={curOption}>{curOption}</option>;
                     })
                    }
                </select>
            </label>
        </div>
    );
}

export default DropDown;