import React, { useState, createContext } from 'react'
import { IAuditorium, IProjection, ICinema, IMovie } from "../../models";



interface IState {
  movies: IMovie[];
  cinemas: ICinema[];
  auditoriums: IAuditorium[];
  filteredAuditoriums: IAuditorium[];
  filteredMovies: IMovie[];
  filteredProjections: IProjection[];
  dateTime: string;
  id: string;
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
  date: Date;
  cinemaId: string;
  auditoriumId: string;
  movieId: string;
  name: string;
}

export const ProjectionContext = createContext({});

export const ProjectionProvider = (props) => {
  
    const [state, setState] = useState<IState>({
        movies: [
          {
            id: "",
            bannerUrl: "",
            title: "",
            rating: 0,
            year: "",
            projections: [
              {
                id: "",
                movieId: "",
                projectionTime: "",
                auditoriumName: "",
              },
            ],
          },
        ],
        cinemas: [
          { id: "",
           name: "",
          }],
        auditoriums: [
          {
            id: "",
            name: "",
          },
        ],
        filteredAuditoriums: [
          {
            id: "",
            name: "",
          },
        ],
        filteredMovies: [
          {
            id: "",
            bannerUrl: "",
            title: "",
            rating: 0,
            year: "",
          },
        ],
        filteredProjections: [
          {
            id: "",
            movieId: "",
            projectionTime: "",
            bannerUrl: "",
            auditoriumName: "",
            movieTitle: "",
            movieRating: 0,
            movieYear: "",
          },
        ],
        cinemaId: "",
        auditoriumId: "",
        movieId: "",
        dateTime: "",
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
        date: new Date(),
      });

      return(
        <ProjectionContext.Provider value={[state, setState]}>
          {props.children}
        </ProjectionContext.Provider>
      );
};