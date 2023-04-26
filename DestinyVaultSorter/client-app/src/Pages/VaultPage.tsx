import React, { useState } from 'react';
import {useTable} from "react-table";
import logo from '../logo.svg';
import '../App.css';
import { Link } from 'react-router-dom';
import Button from '../components/RoundButton'

interface Data{
    icon: string;
    name: string;
    type: string;
    element: string;
    level: number;
    slot: string;
}

function VaultPage() {
    const [data, setData] = useState<Data[]>([]);

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
    )

    // @ts-ignore
    let tableInstance = useTable({columns, data})
    const {
        getTableProps,
        getTableBodyProps,
        headerGroups,
        rows,
        prepareRow,
    } = tableInstance

    async function populateWeapons() {
        const resposne = await fetch("weapons/search?weaponSlot=Heavy");
        const wepData = await resposne.json();
        addDataToTable(wepData)
      }
  
    
    function addDataToTable(wepData: any){
      let newData: any = []
      Object.keys(wepData).forEach(function(key) {
          newData.push({wepIcon: "https://www.bungie.net"+wepData[key].weaponIconLink, wepName: wepData[key].weaponName, wepType: wepData[key].weaponType, wepElement: wepData[key].weaponElement, wepLevel: wepData[key].weaponLevel, wepSlot: wepData[key].weaponSlot});
      })
      setData(newData)
    }

    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <p>
            <Link to="/">Go Home.</Link>
          </p>  
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

  export default VaultPage;