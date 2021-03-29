import {IAuditorium} from '../models/index';
import { ICreateAuditoriumWithCinema } from './AuditoriumModels';

export interface ICinemaToCreateModel
{
name: string;
address:string;
cityName:string;
createAuditoriumModel?:ICreateAuditoriumWithCinema[];
}