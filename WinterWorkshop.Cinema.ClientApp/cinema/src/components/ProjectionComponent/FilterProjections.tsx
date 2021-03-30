import React, { useEffect, useState , memo, useMemo,Dispatch,SetStateAction} from "react";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, Button } from "react-bootstrap";
import "./../../index.css";

import SelectAuditoriums from "./SelectFilters/SelectAuditoriums"
import SelectCinenma from "./SelectFilters/SelectCinemas"
import SelectMovies from "./SelectFilters/SelectMovies"
import * as Service from "./ProjectionService"
import {IMovie, IProjection, IAuditorium,  ICinema} from "../../models";
import {IInfoState,IStateMovies} from "./Projections"
import { classicNameResolver } from "typescript";
import DatePicker from 'DatePicker';
import {cinemaService} from "../Services/cinemaService"
import {auditoriumService} from "../Services/auditoriumService"

interface IProps{
  movies: IMovie[];
  setMovies: Dispatch<SetStateAction<IStateMovies>>;
  info: IInfoState;
  setInfo: Dispatch<SetStateAction<IInfoState>>;
  handleSubmit : (event: React.FormEvent<HTMLFormElement>) => void;
}

interface IFilteredData{
  filteredAuditoriums:IAuditorium[];
  filteredMovies:IMovie[];
}

const FilterProjections:React.FC<IProps> = memo(({movies,setMovies,info,setInfo,handleSubmit}) =>{
    
    const[cinemas,setCinemas]=useState<{cinemas: ICinema[]}>({
      cinemas: []
    });
    const[auditoriums,setAuditoriums]=useState<{auditoriums: IAuditorium[]}>({
      auditoriums: []
    });
    const[filteredData,setFilteredData]=useState<IFilteredData>({
      filteredAuditoriums: [],
      filteredMovies: []
    });
    const [btnColor, onbtnColor] = useState(false);

    useEffect(() => {
    //  Service.getCurrentMoviesAndProjections(setInfo,setMovies);
    //  Service.getAllCinemas(setInfo,setCinemas);
    getAllCinemas();
    getAllAuditoriums();
    //  Service.getAllAuditoriums(setInfo,setAuditoriums);
    console.log("FIlter Data")
    }, []);

    const getAllCinemas = async() =>{
      var data=await cinemaService.getCinemas();
      if(data===undefined){
        return;
      }
      setCinemas({cinemas:data});
    }

    const getAllAuditoriums = async()=>{
      var data=await auditoriumService.getAllAuditoriums();
      if(data===undefined){
        return;
      }
      setAuditoriums({auditoriums:data});
    }


    
    const infoCinema = useMemo(()=>info,[info.selectedCinema,info.selectedAuditorium]);


    console.log("render FILTER");
    return(
        <form 
        id="name"
        name={info.name}
        onSubmit={handleSubmit}
        className="filter"
       >
         <Row>
        <span className="filter-heading align-self-center">Filter by:</span>
        
        <SelectCinenma cinemas={cinemas.cinemas} setInfo={setInfo} setFilteredData={setFilteredData}/>

        <SelectAuditoriums selectedCinema={info.selectedCinema} selectedAuditoriumId={info.auditoriumId} filteredAuditoriums={filteredData.filteredAuditoriums} auditoriums={auditoriums.auditoriums} setInfo={setInfo} setFilteredData={setFilteredData}/>
        
        <SelectMovies info={infoCinema} setInfo={setInfo} filteredMovies={filteredData.filteredMovies} movies={movies}/>
        
          <DatePicker
              type="element"
              // value={info.date}
              onChange={date => {if(date!==null){onbtnColor(true);
                                  setInfo((prev)=>({...prev ,dateTime:date,selectedDate:true}));
                                }else {
                                  onbtnColor(false);
                                  setInfo((prev)=>({...prev ,dateTime:date,selectedDate:false}));}
              }}>
              <button type="button" 
              className={btnColor ? "btn btn-info mr-2": "btn btn-outline-primary mr-2" }>📅</button>
            </DatePicker>
            
       
        <button
          id="filter-button"
          className="btn-search "
          type="submit"
          onClick={() => setInfo((prev)=>({ ...prev, submitted: true }))}
        >
          Submit
        </button>
        </Row>
      </form>
     
    );
});

export default FilterProjections;