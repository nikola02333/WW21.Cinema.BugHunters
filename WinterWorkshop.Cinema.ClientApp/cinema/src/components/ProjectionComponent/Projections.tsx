import React, { useEffect, useState,useMemo } from "react";
import { NotificationManager } from "react-notifications";
import { withRouter } from "react-router-dom";
import { Container, Row } from "react-bootstrap";
import "./../../index.css";
import { IAuditorium, IProjection, IMovie } from "../../models";
import FilterProjections from "./FilterProjections";
import MovieProjectCard from "./MovieProjectionCard";
import * as Service from "./ProjectionService"
import {projectionService} from "../Services/projectionService"
import {movieService} from "../Services/movieService"
import {IFilterProjectionModel} from "../../models/ProjectionModels"
export interface IStateMovies{
  movies:IMovie[];
}
interface IProjectionState{
  currantProjections:IProjection[];
  filteredProjections:IProjection[];
}

export interface IInfoState{
  cinemaId: number;
  auditoriumId: number;
  movieId: string;
  dateTime: Date;
  id: string;
  name: string;
  current: boolean;
  tag: string;
  titleError: string;
  yearError: string;
  submitted: boolean;
  isLoading: boolean;
  selectedCinema: boolean;
  selectedAuditorium: boolean;
  selectedMovie: boolean;
  selectedDate: boolean;
}

const Projections : React.FC = (props: any) => {

  const [movies,setMovies]=useState<IStateMovies>({
    movies: [{
      year:"0",
      rating:0,
      title:"",
      id:"",
      coverPicture:"",
      tagsModel:[{
        tagId:0,
        tagName:"",
        tagValue:""
      }]
    }]
  });

  const [projection,setProjection]=useState<IProjectionState>({
    currantProjections:[],
    filteredProjections: [],
  });

  const [info, setInfo] = useState<IInfoState>({
    cinemaId: 0,
    auditoriumId: 0,
    movieId: "",
    dateTime: new Date(0),
    id: "",
    name: "",
    current: false,
    tag: "",
    titleError: "",
    yearError: "",
    submitted: false,
    isLoading: true,
    selectedCinema: false,
    selectedAuditorium: false,
    selectedMovie: false,
    selectedDate: false,
  });

    useEffect(()=>{
      getMovies();
      getCurrentProjections();
    },[]);

    const getCurrentProjections= async() =>{
      setInfo((prev)=>({ ...prev, isLoading: true }));
      var projections =await projectionService.getAllProjections(true);
      if(projections===undefined){
        setInfo((prev)=>({ ...prev, isLoading: false }));
        return;
      }
      setProjection((prev)=>({...prev, currantProjections:projections}));
      setInfo((prev)=>({ ...prev, isLoading: false }));

    }

    const getMovies = async() =>{

      var movies = await movieService.getAllMovies();
      if(movies===undefined){
        return;
      }
      setMovies((prev)=>({...prev, movies:movies}));
    }

    const getFilteredProjections = async() =>{
      var filter : IFilterProjectionModel= {
        AuditoriumId:info.auditoriumId,
        CinemaId:info.cinemaId,
        DateTime:info.dateTime,
        MovieId:info.movieId
      };

      var filteredProjections =await projectionService.getFilteredProjection(filter);
      debugger;
      if(filteredProjections===undefined || filteredProjections.length===0){
        NotificationManager.info("No projections for given filter");
        setInfo((prev)=>({ ...prev, submitted: false,selectedDate: false,dateTime:new Date(0)}));
        return;
      }
      let allMovies = movies.movies;

      for (let i = 0; i < allMovies.length; i++) {
        for (let j = 0; j < filteredProjections.length; j++) {
          if (allMovies[i].id === filteredProjections[j].movieId) {
            filteredProjections[j].coverPicture = allMovies[i].coverPicture;
          }
        }
      }
      setProjection((prev)=>({...prev, filteredProjections: filteredProjections}));

    }

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
      
      event.preventDefault();
      const { cinemaId, auditoriumId, movieId, dateTime } = info;
      if (cinemaId || auditoriumId || movieId  ||( dateTime && dateTime.getFullYear()!==1970) ) {
        getFilteredProjections();
      } else {
         if(!cinemaId && !auditoriumId && !movieId && ( !dateTime || dateTime.getFullYear()===1970) ){
          NotificationManager.info("Current projections");
        }else{
          NotificationManager.error("Not found.");
        }
        
        setInfo((prev)=>({ ...prev, submitted: false }));
      }
    };


    const infoMemo = useMemo(()=>info.submitted,[info.submitted,projection.filteredProjections]);
    const moviesMemo = useMemo(()=>movies.movies,[movies.movies]);
    const filterProjectionsMemo = useMemo(()=>projection.filteredProjections,[projection.filteredProjections]);
    const currantProjectionsMemo = useMemo(()=>projection.currantProjections,[projection.currantProjections]);
   
    return (
        <Container>
          <Row className="justify-content-center mt-3">
          <h1 className="projections-title">{info.submitted?"Filtered projections":"Current projections"}</h1>
          </Row>
         
          <Row className="justify-content-center">
          <FilterProjections handleSubmit={handleSubmit} movies={moviesMemo} setMovies={setMovies} info={info} setInfo={setInfo}/>
          </Row>
          <MovieProjectCard submitted={infoMemo} movies={moviesMemo} currantProjections={currantProjectionsMemo} filteredProjections={filterProjectionsMemo}/>
          
        </Container>
      );
};

export default withRouter(Projections);