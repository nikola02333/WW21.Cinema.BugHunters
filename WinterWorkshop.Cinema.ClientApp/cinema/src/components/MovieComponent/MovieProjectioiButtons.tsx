import React from 'react'
import {groupProjectionButton,groupedProjectionButtons} from "../helpers/functions"
import {Col} from "react-bootstrap";
import {IMovie, IProjection,IGroupedProjections} from "../../models/index"


interface IProps{
    groupedProjections:IGroupedProjections;
    movie:IMovie;
    history
}


const MovieProjectionButtons : React.FC<IProps> =({groupedProjections,movie,history})=>{
    
    const buttons=(groupedProjections, movie, history)=>{
        if(groupedProjections!=={} && movie!=={} ){
            groupedProjectionButtons(groupedProjections, movie, history)
        }else{
            return (
                <></>
            );
        }
    }

    return(
        <Col xs={12}>
                    {buttons(groupedProjections, movie, history)}
        </Col>

    );
    
}
export default MovieProjectionButtons;