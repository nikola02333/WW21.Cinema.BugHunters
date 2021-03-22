export interface IProjection {
  id: string;
  projectionTime: string;
  movieId: string;
  auditoriumName: string;
  coverPicture?: string;
  movieTitle?: string;
  movieRating?: number;
  movieYear?: string;
}
export interface IProjectionNEW {
  auditoriumId: number;
  auditoriumName: string;
  duration: number;
  id: string;
  movieId: string;
  movieTitle: string;
  price: number;
  projectionTime: string;
}

export interface IMovie {
  id: string;
  title: string;
  rating: number;
  year: string;
  coverPicture?: string;
  current?: boolean;
  hasOscar?:boolean;
  projections?: IProjection[];
}

export interface ICinema {
  id: string;
  name: string;
}

export interface IAuditorium {
  id: string;
  name: string;
  cinemaId?: string;
}

export interface ISeats {
  id: string;
  number: number;
  row: number;
}

export interface ICurrentReservationSeats {
  currentSeatId: string;
}

export interface IReservedSeats {
  seatId: string;
}

export interface IUser {
  id: string;
  firstName: string;
  lastName: string;
  bonusPoints: string;
}

export interface IReservation {
  projectionId: string;
}

export interface ITag {
  name: string;
}
