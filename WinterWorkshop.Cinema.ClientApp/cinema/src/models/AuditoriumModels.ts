export interface ICreateAuditorium{
    cinemaId? : string;
    auditoriumName: string;
    seatRows : number;
    numberOfSeats : number;
}

export interface ICreateAuditoriumWithCinema{
    id:number;
    cinemaId? : string;
    auditoriumName: string;
    seatRows : number;
    numberOfSeats : number;
}

export interface IAuditoriumToUpdate{
    auditoriumName: string;   
}