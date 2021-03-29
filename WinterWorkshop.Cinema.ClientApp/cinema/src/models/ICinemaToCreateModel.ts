import {IAuditorium} from '../models/index';

export interface ICinemaToCreateModel
{
name: string;
address:string;
cityName:string;
createAuditoriumModel?:IAuditorium;
}