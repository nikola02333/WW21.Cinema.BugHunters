export interface IFilterProjectionModel{
    CinemaId?: number,
    AuditoriumId?: number,
    MovieId?: string,
    DateTime?: Date
}

export interface ICreateProjection{
    AuditoriumId:number,
    ProjectionTime:Date,
    Duration:number,
    MovieId:string,
    Price:number
}

