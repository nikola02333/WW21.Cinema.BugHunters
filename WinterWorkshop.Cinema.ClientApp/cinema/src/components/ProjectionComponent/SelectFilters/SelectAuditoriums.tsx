import React,{memo} from 'react'
import { movieService } from '../../Services/movieService';


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
      const MoviesBySelectedAuditorium = async(id:string) =>{
        if(id!=="none"){
          
        props.setInfo((prev)=>({...prev, auditoriumId: +id }));
        var data =await movieService.getMovieByAuditoriumId(+id);
        if(data===undefined){
          return;
        }
        console.log(data);
        props.setFilteredData((prev)=>({
          ...prev,
          filteredMovies: data
        }))
        props.setInfo((prev)=>({
          ...prev,
          isLoading: false,
          selectedAuditorium: true,
        }));
      }else{
        props.setInfo((prev)=>({
          ...prev,
          auditoriumId: "",
          selectedAuditorium: false
        }))
      }
       
    }
    return(
        <select
        onChange={(e) => MoviesBySelectedAuditorium(e.target.value)}
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