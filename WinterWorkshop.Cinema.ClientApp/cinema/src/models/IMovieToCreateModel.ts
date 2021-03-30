export interface IMovieToCreateModel
{
    Title: string;
      Year: number;
      Current: boolean;
      Rating: number;
      Tags: string;
      CoverPicture: string;
      Genre: string;
      Actors: string;
      Description:string;
      ImdbId?: string;
      HasOscar:boolean;
}