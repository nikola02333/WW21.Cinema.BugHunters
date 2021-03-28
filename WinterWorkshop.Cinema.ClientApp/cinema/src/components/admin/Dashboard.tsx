import * as React from "react";
import { Row, Col } from "react-bootstrap";
import { Switch, Route } from "react-router-dom";
import "./../../index.css";

// Admin actions
import NewMovie from "../MovieComponent/NewMovie";
import EditMovie from "../MovieComponent/EditMovie";
//import ShowAllMovies from "../MovieComponent/ShowAllMovies";
import TopTenMovies from "./MovieActions/TopTenMovies";
import NewCinema from "./CinemaActions/NewCinema";
import EditCinema from "./CinemaActions/EditCinema";
import ShowAllCinemas from "./CinemaActions/ShowAllCinemas";
import NewAuditorium from "./AuditoriumActions/NewAuditorium";
import ShowAllAuditoriums from "./AuditoriumActions/ShowAllAuditoriums";
import ShowAllProjections from "./ProjectionActions/ShowAllProjections";
import NewProjection from "./ProjectionActions/NewProjection";
import ProjectionDetails from "./../user/ProjectionDetails";
import UserProfile from "./../user/UserProfile";
import Projection from "../user/Projection";
import EditAuditorium from "./AuditoriumActions/EditAuditorium";
import EditProjection from "./ProjectionActions/EditProjection";
import { checkRole } from "../helpers/roleCheck";
import { PrivateRouteAdminAndSuperUser } from "../hoc/privateRouteAdminAndSuperUser";
import { PrivateRouteAdmin } from "../hoc/privateRouteAdmin";
import { PrivateRouteAdminSuperUserAndUser } from "../hoc/privateRouteAdminSuperUserAndUser";
import Projections from "./../ProjectionComponent/Projections";
import TicketReservation from "./../TicketReservationConmponent/TicketReservation"
import Movies from './../MovieComponent/Movies';
import MovieDeatails from './../MovieComponent/MovieDetails';

// higher order component
const Dashboard: React.FC = () => {
  
  return (
    <Row className="justify-content-center no-gutters">
      {checkRole()}
      <Col lg={10} className="pt-2 app-content-main">
        <Switch>
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/NewMovie"
            component={NewMovie}
          />
          <Route path="/dashboard/AllMovies" component={Movies} />
          <Route path="/dashboard/TopTenMovies" component={TopTenMovies} />
         
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/EditMovie/:id"
            component={EditMovie}
          />
           <PrivateRouteAdmin
            path="/dashboard/NewCinema"
            component={NewCinema}
          /> 
          <PrivateRouteAdmin
            path="/dashboard/AllCinemas"
            component={ShowAllCinemas}
          />
          <PrivateRouteAdmin
            path="/dashboard/EditCinema/:id"
            component={EditCinema}
          />
         <PrivateRouteAdmin
            path="/dashboard/NewAuditorium"
            component={NewAuditorium}
          /> 
          <PrivateRouteAdmin
            path="/dashboard/EditAuditorium"
            component={EditAuditorium}
          />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/AllAuditoriums"
            component={ShowAllAuditoriums}
          />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/AllProjections"
            component={ShowAllProjections}
          />
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/NewProjection"
            component={NewProjection}
          />
          <Route
            path="/dashboard/ProjectionDetails"
            component={ProjectionDetails}
          />
          <Route path="/dashboard/Projection" component={Projection} />
         
          <Route path="/dashboard/Projections" component={Projections} />
          <Route path="/dashboard/TicketReservation" component={TicketReservation} />
          <Route path="/dashboard/MovieDetails" component={MovieDeatails} />
          
          
          <PrivateRouteAdminAndSuperUser
            path="/dashboard/EditProjection"
            component={EditProjection}
          />
          <PrivateRouteAdminSuperUserAndUser
            path="/dashboard/UserProfile"
            component={UserProfile}
          />
        </Switch>
      </Col>
    </Row>
  );
};

export default Dashboard;
