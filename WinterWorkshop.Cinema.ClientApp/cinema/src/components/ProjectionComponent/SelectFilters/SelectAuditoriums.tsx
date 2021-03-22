import React,{memo} from 'react'
import {getMoviesBySelectedAuditorium} from "../ProjectionService"

const SelectAuditoriums  = memo( (props:{selectedAuditoriumId,selectedCinema,filteredAuditoriums,auditoriums,setInfo,setFilteredData}) =>{
    console.log("FilterAuditoriums")

    var auditoriumIsSame = false;
    const fillFilterWithAuditoriums = () => {
        if (props.selectedCinema) {
          const filterAuditoriums =  props.filteredAuditoriums.map((auditorium) => {
            if(auditorium.id==props.selectedAuditoriumId){
              auditoriumIsSame=true;
              console.log("TRUE");
            }
            return <option key={auditorium.id} value={auditorium.id}>{auditorium.name}</option>;
          });
          if(!auditoriumIsSame){
            props.setInfo((prev)=>({...prev,auditoriumId: "",selectedAuditorium: false}));
          }  
          return filterAuditoriums;
        } else {
          return props.auditoriums.map((auditorium) => {
            return (
              <option   value={auditorium.id} key={auditorium.id}>
                {auditorium.name}
              </option>
            );
          });
        }
      };
    return(
        <select
        onChange={(e) => getMoviesBySelectedAuditorium(e.target.value , props.setInfo, props.setFilteredData)}
        name="auditoriumId"
        id="auditorium"
        className="select-dropdown"
        
      >
        <option value="none">Auditorium</option>
        {fillFilterWithAuditoriums()}
      </select>
    );
});
export default SelectAuditoriums;