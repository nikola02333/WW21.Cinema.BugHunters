import React ,{useEffect,useState,useCallback}from 'react'
import { Container, Row, Col, Button ,Image} from "react-bootstrap";
import {movieService} from "../Services/movieService";
import {IMovie,IProjection} from "../../models/index";
import {IMDB,imdbData} from "../../models/IMBDModel"
import {getRoundedRating} from "../helpers/functions";
import { withRouter } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faStar } from '@fortawesome/free-solid-svg-icons'
import {groupProjectionButton,groupedProjectionButtons} from "../helpers/functions"
import { projectionService } from '../Services/projectionService';
import {IFilterProjectionModel} from "../../models/ProjectionModels"
import MovieProjectionButtons from "./MovieProjectioiButtons"
import ReactPlayer from 'react-player'


 interface IProjectionByCinemaId {
    cinema: IProjection[];
  }
  interface IGroupedProjections{
    movieId:string;
    projections:IProjectionByCinemaId[];
  }


const MovieDeatails : React.FC =(props: any)=>{
var id = window.location.pathname.split("/")[3];

const [movie,setMovie]= useState<IMovie>({
    id: "",
    coverPicture: "",
    title: "",
    year: "",
    rating: 0,
});
const [imbd,setImdb]=useState<IMDB>(imdbData);
const [projections,setProjections]= useState<IProjection[]>([]);
const [groupedProjections,setGroupedProjections]=useState<IGroupedProjections>(
    {
        movieId: "",
        projections: [
            {
                cinema: [
                    {
                        id: "",
                        movieTitle: "",
                        auditoriumId: 0,
                        auditoriumName: "",
                        projectionTime: "",
                        duration: 0,
                        price: 0,
                        cinemaId: 0,
                        cinemaName: "",
                        movieId: ""
                    }
                ]
            }
            
        ]
    
});

    useEffect(()=>{
        getMovie();
        getProjection(id);
        
    },[])
    
    const grouped = (projections:IProjection[])=>{
        const grouped = groupProjectionButton(projections);
        setGroupedProjections(grouped);
    }

    const getProjection = async(id:string)=>{
        const filter:IFilterProjectionModel={
            MovieId:id
        }
        const projection= await projectionService.getFilteredProjection(filter);
        if(projection===undefined){
            return;
        }

        setProjections(projection);
        grouped(projection);
    }

    const getMovie= async()=>{
        const movie= await movieService.getMovieById(id) ;
        if(movie===undefined){
            return;
        }
        console.log(movie);
        setMovie(movie);
    }
    

    return(
        <Container key={movie.id}>
            <Col xs={12}  className="shadow rounded my-3">
            <Row className="justify-content-center">
              <Col sm={3} className=" my-4 ml-1 d-flex justify-content-center">
              <Image  className="img-responsive img-fluid" style={{  borderRadius:5}} src={imbd.data.image} />
              </Col>
              <Col md={7} className=" my-3">
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">Name:</span>
                    </Col>
                    <Col>
                    <span className="">{imbd.data.fullTitle}</span>
                    </Col>
                </Row >
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">Producer:</span>
                    </Col>
                    <Col>
                    <span className="">{imbd.data.directors}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">Genre:</span>
                    </Col>
                    <Col>
                    <span className="">{imbd.data.genres}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">Stars:</span>
                    </Col>
                    <Col>
                    <span className="">{imbd.data.stars}</span>
                    </Col>
                </Row >
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">Runtime:</span>
                    </Col>
                    <Col>
                    <span className="">{imbd.data.runtimeStr}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">Year:</span>
                    </Col>
                    <Col>
                    <span className="">{imbd.data.year}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">Awards:</span>
                    </Col>
                    <Col>
                    <span className="">{imbd.data.awards}</span>
                    </Col>
                </Row>
                <Row className="mt-3">
                    <Col xs={3}>
                    <span className="font-weight-bold movie-details">IMDB rating:</span>
                    </Col>
                    <Col>
                    <span className=""><FontAwesomeIcon icon={faStar} className="text-warning"/>{imbd.data.imDbRating+"/10"}</span>
                    </Col>
                </Row>
              </Col>
            </Row>
            {/* <Row>
                <MovieProjectionButtons groupedProjections={groupedProjections} movie={movie} history={props.history} />
            </Row> */}
            
            
            <Row className="justify-content-center my-3 ">
                <Col xs={10} className="shadow-sm rounded">
                
                <Col xs={12} className="">
                    <p className="movie-detail  text-center m-0">Plot:</p>
                </Col>
                <Col xs={12} className="my-2 " >
                    <p className="text-justify ">{imbd.data.plot}</p>
                </Col>
                </Col>
            </Row>
            <Row className=" justify-content-center my-2 ">
                <Col  className="d-flex justify-content-center my-2 ">
                <ReactPlayer controls url={imbd.youtubeData.videoUrl}/>
                </Col>
            </Row>
            </Col>
        </Container>
    )
}
export default withRouter(MovieDeatails);