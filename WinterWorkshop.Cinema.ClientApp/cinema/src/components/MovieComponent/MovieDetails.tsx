import React ,{useEffect,useState}from 'react'
import { Container, Row, Col, Card, Button ,Image,Table,OverlayTrigger,Tooltip} from "react-bootstrap";
import {movieService} from "../Services/movieService";
import {IMovie} from "../../models/index";
import {IMDB} from "../../models/IMBDModel"
import {getRoundedRating} from "../helpers/functions";
import { withRouter } from "react-router-dom";


interface IState{
    movie:IMovie;
    IMDBData:IMDB;
}

const MovieDeatails : React.FC =()=>{
    
const [movie,setMovie]= useState<IMovie>();


    useEffect(()=>{
        getMovie();
    },[])
    useEffect(()=>{
        console.log(movie);
    },[movie]);

    const getMovie= async()=>{
        var id = window.location.pathname.split("/")[3];
        const movie= await movieService.getMovieById(id) ;
        if(movie===undefined){
            return;
        }
        setMovie(movie);
    }

    return(
        <Container key={movie?.id} className="shadow rounded my-3">
            <Row >
              <Col md={3} className="  my-3">
              <Image  className=" img-responsive img-fluid" style={{  borderRadius:5}} src={movie?.coverPicture} />
              </Col>
              <Col md={9} className=" my-3">
                <Col md={12}>
                <span className="card-title-font">{movie?.title}</span>
                 {getRoundedRating(movie?.rating as number)}
                </Col>
  
                <Col md={12} className="mb-2 text-muted">
                Year of production: {movie?.year}
                </Col>
              </Col>
            </Row>
            </Container>
    )
}
export default withRouter(MovieDeatails);