import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../appSettings";


export const getCurrentMoviesAndProjections = (setInfo,setMovies) => {
    // setInfo((prev)=>({ ...prev, submitted: false }));

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setInfo((prev)=>({ ...prev, isLoading: true }));
    fetch(
      `${serviceConfig.baseURL}/api/movies/AllMovies/${true}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setInfo((prev)=>({ ...prev, movies: data, isLoading: false }));
          setMovies((prev)=>({ ...prev, movies: data }));
        }
        
      })
      .catch((response) => {
        setInfo((prev)=>({ ...prev, isLoading: false }));
        NotificationManager.error(response.message || response.statusText);
      });
  };

export const getAllCinemas = (setInfo,setCinemas) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };
    setInfo((prev)=>({ ...prev, isLoading: true }));
    fetch(`${serviceConfig.baseURL}/api/Cinemas/all`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setInfo((prev)=>({
            ...prev,
            isLoading: false
          }));
          setCinemas({
            cinemas : data
          });
      }})
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setInfo((prev)=>({ ...prev, isLoading: false }));
      });
  };

  export const getAllAuditoriums = (setInfo,setAuditoriums) => {
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setInfo((prev)=>({ ...prev, isLoading: true }));
    fetch(`${serviceConfig.baseURL}/api/auditoriums/all`, requestOptions)
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setInfo((prev)=>({ ...prev, isLoading: false }));
          setAuditoriums({auditoriums: data} );
        }
        
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setInfo((prev)=>({ ...prev, isLoading: false }));
      });
  };


  export const getAuditoriumsBySelectedCinema = (selectedCinemaId: string, setInfo, setFilteredData) => {
    if(selectedCinemaId!=="none"){
    setInfo((prev)=>({ ...prev, cinemaId: selectedCinemaId }));

    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setInfo((prev)=>({...prev, isLoading: true }));
    fetch(
      `${serviceConfig.baseURL}/api/Auditoriums/bycinemaid/${selectedCinemaId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
            setFilteredData((prev)=>({
            ...prev,
            filteredAuditoriums: data,
          }));
          setInfo((prev)=>({
            ...prev,
            isLoading: false,
            selectedCinema: true
          }))
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setInfo((prev)=>({...prev, isLoading: false }));
      });
    }else{
      setInfo((prev)=>({
        ...prev,
        cinemaId: "",
        selectedCinema: false
      }))
    }
  };

  export const getMoviesBySelectedCinema = (selectedCineamId: string, setInfo, setFilteredData) => {
    if(selectedCineamId!=="none"){
    setInfo((prev)=>({...prev, cinemaId: selectedCineamId }));
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setInfo((prev)=>({...prev, isLoading: true }));
    fetch(
      `${serviceConfig.baseURL}/api/Movies/bycineamId/${selectedCineamId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setFilteredData((prev)=>({
            ...prev,
            filteredMovies: data
          }));
          setInfo((prev)=>({
            ...prev,
            isLoading: false,
            selectedCinema: true,
          }));
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setInfo((prev)=>({...prev, isLoading: false }));
      });
    }else{
      setInfo((prev)=>({
        ...prev,
        cinemaId: "",
        selectedCineam: false
      }))
    }
  };

  export const getMoviesBySelectedAuditorium = (selectedAuditoriumId: string, setInfo, setFilteredData) => {
    if(selectedAuditoriumId!=="none"){
    setInfo((prev)=>({...prev, auditoriumId: selectedAuditoriumId }));
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setInfo((prev)=>({...prev, isLoading: true }));
    fetch(
      `${serviceConfig.baseURL}/api/Movies/byauditoriumid/${selectedAuditoriumId}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }
        return response.json();
      })
      .then((data) => {
        if (data) {
          setFilteredData((prev)=>({
            ...prev,
            filteredMovies: data
          }))
          setInfo((prev)=>({
            ...prev,
            isLoading: false,
            selectedAuditorium: true,
          }));
        }
      })
      .catch((response) => {
        NotificationManager.error(response.message || response.statusText);
        setInfo((prev)=>({...prev, isLoading: false }));
      });
    }else{
      setInfo((prev)=>({
        ...prev,
        auditoriumId: "",
        selectedAuditorium: false
      }))
    }
  };

 export const getCurrentFilteredMoviesAndProjections = (info,setInfo,moviesState,setProjections) => {
    const { cinemaId, auditoriumId, movieId, dateTime } = info;
    const requestOptions = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
      },
    };

    setInfo((prev)=>({ ...prev, isLoading: true }));
    let query = "";
    if (cinemaId) {
      query = `cinemaId=${cinemaId}`;
    }
    if (auditoriumId) {
      query += `${query.length ? "&" : ""}auditoriumId=${auditoriumId}`;
    }
    if (movieId) {
      query += `${query.length ? "&" : ""}movieId=${movieId}`;
    }
    if (dateTime && dateTime.getFullYear()!==1970 ) {
      query += `${query.length ? "&" : ""}dateTime=${dateTime.toISOString()}`;
    }
    if (query.length) {
      query = `?${query}`;
    }
    fetch(
      `${serviceConfig.baseURL}/api/projections/filter${query}`,
      requestOptions
    )
      .then((response) => {
        if (!response.ok) {
          return Promise.reject(response);
        }

        return response.json();
      })
      .then((data) => {
        if (data) {
          let movies = moviesState.movies;
          let filteredMovies = data;

          for (let i = 0; i < movies.length; i++) {
            for (let j = 0; j < filteredMovies.length; j++) {
              if (movies[i].id === data[j].movieId) {
                data[j].coverPicture = movies[i].coverPicture;
              }
            }
          }
         
          setProjections({filteredProjections: data});
        }
      })
      .catch((response) => {
        setInfo((prev)=>({ ...prev, isLoading: false }));
        NotificationManager.error(response.message || response.statusText);
      });
  };

  