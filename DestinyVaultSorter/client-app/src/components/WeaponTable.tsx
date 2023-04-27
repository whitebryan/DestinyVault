import React, { useState, useEffect} from 'react';
import {TableInstance, useTable} from "react-table";

interface tableProps{
    page: number,
    setPage: (dir: number) => void
    type: string,
    element: string,
    slot: string,
    level: number,
}

interface Data{
    icon: string;
    name: string;
    type: string;
    element: string;
    level: number;
    slot: string;
}

function clampNumber(num: number, min: number, max: number): number{
    return Math.min(Math.max(num, min), max);
}

function WeaponTable(props: tableProps){
    const [curInd, setCurInd] = useState(0);
    const [data, setData] = useState<Data[]>([]);
    const [fullData, setFullData] = useState<Data[]>([]);


    function changePage(direction: number): void{
        if(direction > 0 && data.length === 5){
            let newInd = clampNumber(curInd + 5, 0, fullData.length - 1);
            setCurInd(newInd);
            props.setPage(0);
        }
        else if(direction < 0){
            let newInd = clampNumber(curInd - 5, 0, fullData.length - 1);
            setCurInd(newInd);
            props.setPage(0);
        }
    }

    let columns = React.useMemo(
        () => [
            {
                Header: 'Icon',
                accessor: 'wepIcon',
                Cell: (tableProps: { row: { original: { wepIcon: string | undefined; }; }; }) => { return <img src={tableProps.row.original.wepIcon} width={60} alt='Player'/>}
            },
            {
                Header: 'Name',
                accessor: 'wepName',
            },
            {
                Header: 'Type',
                accessor: 'wepType',
            },
            {
                Header: 'Element',
                accessor: 'wepElement',
            },
            {
                Header: 'Level',
                accessor: 'wepLevel',
            },
            {
                Header: 'Slot',
                accessor: 'wepSlot',
            },
        ],
        []
    );

    // @ts-ignore
    let tableInstance = useTable({columns, data})
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
    } = tableInstance

    function addDataToTable(wepData: any){
        let newData: any = []
        Object.keys(wepData).forEach(function(key) {
            newData.push({wepIcon: "https://www.bungie.net"+wepData[key].weaponIconLink, wepName: wepData[key].weaponName, wepType: wepData[key].weaponType, wepElement: wepData[key].weaponElement, wepLevel: wepData[key].weaponLevel, wepSlot: wepData[key].weaponSlot});
        })
        setFullData(newData)
        setData(newData.slice(curInd, curInd+5))
    }

    async function populateWeapons() {
        let requestString = "weapons/search?"
        if(props.type != "Any")
            requestString = requestString + "weaponType="+props.type+'&';
        if(props.element != "Any")
            requestString = requestString + "weaponElement="+props.element+'&';
        if(props.slot != "Any")
            requestString = requestString + "weaponSlot="+props.slot+'&';
        if(props.level)
            requestString = requestString + "weaponLevel="+props.level+'&';

        const resposne = await fetch(requestString);
        const wepData = await resposne.json();
        addDataToTable(wepData)
    }

    useEffect(() => {
        populateWeapons();
    }, [props.element, props.level, props.slot, props.type]);// eslint-disable-line react-hooks/exhaustive-deps

    useEffect(() => {
        changePage(props.page);
    }, [props.page]);// eslint-disable-line react-hooks/exhaustive-deps

    useEffect(() => {
        setData(fullData.slice(curInd, curInd+5));
    }, [curInd] );// eslint-disable-line react-hooks/exhaustive-deps

    return(
        <div>
            <table {...getTableProps()}>
            <thead>
                {
                    headerGroups.map(headerGroup => (
                        <tr {...headerGroup.getHeaderGroupProps()}>
                            {
                                headerGroup.headers.map(column => (
                                    <th {...column.getHeaderProps()}>
                                        {
                                            column.render('Header')
                                        }
                                    </th>
                                ))
                            }
                        </tr>
                    ))
                }      
            </thead>      
            <tbody {...getTableBodyProps()}>
                {
                    rows.map(row => {
                        prepareRow(row)
                        return (
                            <tr {...row.getRowProps()}>
                                {
                                    row.cells.map(cell => {
                                        return (
                                            <td {...cell.getCellProps()}>
                                                {
                                                    cell.render('Cell')
                                                }
                                            </td>
                                        )
                                    })
                                }
                            </tr>
                        )
                    })
                }
            </tbody>
          </table>  
        </div>
    );
}

export default WeaponTable;
