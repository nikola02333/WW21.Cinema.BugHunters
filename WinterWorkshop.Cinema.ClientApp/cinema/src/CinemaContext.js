import React, { createContext, useContext, useState } from "react";

export const CinemaContext = createContext();

export const MovieProvider = (props) => {
  const [movies, setMovies] = useState([
    {
      name: "Movie1",
      price: 123,
      id: 22,
    },
    {
      name: "Movie",
      price: 345,
      id: 33,
    },
  ]);
  return (
    <CinemaContext.Provider value={[movies, setMovies]}>
      {props.children}
    </CinemaContext.Provider>
  );
};
