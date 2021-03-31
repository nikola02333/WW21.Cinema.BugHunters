import React,{memo, useState} from 'react'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faShoppingCart, faCouch,faSpinner } from "@fortawesome/free-solid-svg-icons";
import { Container, Row, Col,Button , Modal} from "react-bootstrap";
import "./../../index.css";
import { useHistory } from "react-router-dom";
import PopUpModel from "./PopUpModel"
import {IStateTicketReservation} from "./TicketReservation"
import Spinner from "../Spinner"

interface IInfo{
  userId: string,
  submitted: boolean,
  projectionPrice:number
}
interface IProps{
  seat:IStateTicketReservation,
  setSeat: React.Dispatch<React.SetStateAction<IStateTicketReservation>>,
  tryReservation(e) : void
  info:IInfo
}
export interface IStateShowAuditoriums{
  modalShow:boolean,
  separately:boolean
}


const ShowAuditorium: React.FC<IProps> = ({info,seat,setSeat,tryReservation}) => {

    const [state,setState]=useState<IStateShowAuditoriums>({
      modalShow:false,
      separately:false
    });
    

    const history = useHistory();
    
    let allButtons: any;

    const renderRows = (maxRoows: number, maxSeats: number) => {
    
        const rowsRendered: JSX.Element[] = [];

        if (seat.seats.length > 0) {
          for (let i = 0; i < maxRoows; i++) {
            const startingIndex = i * maxSeats;
            const maxIndex = (i + 1) * maxSeats;
            
            rowsRendered.push(
              <tr key={i}>{renderSeats(maxSeats, i, startingIndex, maxIndex)}</tr>
            );
          }
        }
        return rowsRendered;
      };

    const checkIfSeatIsTaken = (currentSeatId: string) => {
        return seat.reservedSeats.some(seat=>seat.id===currentSeatId);
      };
    
      const checkIfSeatIsCurrentlyReserved = (currentSeatId) => {
        return seat.currentReservationSeats.some(reserved=>reserved.currentSeatId === currentSeatId);
      };
    
      const getSeatByPosition = (row: number, number: number) => {
        for (let i = 0; i < seat.seats.length; i++) {
          if (seat.seats[i].number === number && seat.seats[i].row === row) {
            return seat.seats[i];
          }
        }
      };
    
      const getSeatById = (seatId: string) => {
        for (let i = 0; i < seat.seats.length; i++) {
          if (seat.seats[i].id === seatId) {
            return seat.seats[i];
          }
        }
      };
    
      const getAllButtons = () => {
        if (!allButtons) {
          allButtons = document.getElementsByClassName("seat");
          for (let i = 0; i < allButtons.length; i++) {
            let seat = getSeatById(allButtons[i].value);
          }
        }
      };
    
      const markSeatAsGreenish = (seatId: string) => {
        getAllButtons();
        for (let i = 0; i < allButtons.length; i++) {
          if (seatId === allButtons[i].value) {
            allButtons[i].className = "seat nice-green-color";
          }
        }
      };
    
      const getButtonBySeatId = (seatId: string) => {
        getAllButtons();
        for (let i = 0; i < allButtons.length; i++) {
          if (seatId === allButtons[i].value) {
            return allButtons[i];
          }
        }
      };
    
      const markSeatAsBlue = (seatId: string) => {
        getAllButtons();
        for (let i = 0; i < allButtons.length; i++) {
          if (seatId === allButtons[i].value) {
            allButtons[i].className = "seat";
          }
        }
      };
    
      const markWholeRowSeatsAsBlue = () => {
        getAllButtons();
        for (let i = 0; i < allButtons.length; i++) {
          if (allButtons[i].className !== "seat seat-taken") {
            allButtons[i].className = "seat";
          }
        }
      };

    const renderSeats = (
    seats: number,
    row: number,
    startingIndex: number,
    maxIndex: number
    ) => {
    let renderedSeats: JSX.Element[] = [];
    let seatIndex = startingIndex;

    if (seat.seats.length > 0) {
        for (let i = 0; i < seats; i++) {
          
          let currentSeatId = seat.seats[seatIndex].id;
          let currentlyReserved = seat.currentReservationSeats.some((seat) => seat.currentSeatId === currentSeatId);
          let seatTaken = seat.reservedSeats.some((seat) => seat.id === currentSeatId);
         
        renderedSeats.push(
          <td key={`row${row}-seat${i}`}>
            <button
            onClick={(e) => {
              e.preventDefault();
                let currentRow = row;
                let currentNumber = i;
                let currSeatId = currentSeatId;
                
                let leftSeatIsCurrentlyReserved = false;
                let leftSeatIsTaken = false;
                let rightSeatIsCurrentlyReserved = false;
                let rightSeatIsTaken = false;
                let leftSeatProperties = getSeatByPosition(currentRow + 1,currentNumber);
                let rightSeatProperties = getSeatByPosition(currentRow + 1, currentNumber + 2);
                let currentReservationSeats = seat.currentReservationSeats;

                if (leftSeatProperties) {
                    leftSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(leftSeatProperties.id);
                    
                    leftSeatIsTaken = checkIfSeatIsTaken(leftSeatProperties.id);
                }
                if (rightSeatProperties) {
                    rightSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(rightSeatProperties.id);
                    rightSeatIsTaken = checkIfSeatIsTaken(rightSeatProperties.id);
                }

                if (!checkIfSeatIsCurrentlyReserved(currSeatId)) {
                    if (seat.currentReservationSeats.length !== 0 && 
                        getButtonBySeatId(currentSeatId).className !=="seat nice-green-color" && !state.separately) {
                          setState((prev)=>({...prev, modalShow:true}));
                        return;
                    }

                    if (!leftSeatIsCurrentlyReserved && !leftSeatIsTaken && leftSeatProperties) {
                    markSeatAsGreenish(leftSeatProperties.id);
                    }

                    if (!rightSeatIsCurrentlyReserved && !rightSeatIsTaken && rightSeatProperties) {
                    markSeatAsGreenish(rightSeatProperties.id);
                    }

                    if (seat.currentReservationSeats.includes({currentSeatId}) === false) {
                    currentReservationSeats.push({
                         currentSeatId,
                    });

                    }
                } else {
                    if (leftSeatIsCurrentlyReserved && rightSeatIsCurrentlyReserved ) {
                        markWholeRowSeatsAsBlue();
                        currentReservationSeats = [];
                    } else {
                        currentReservationSeats.splice(currentReservationSeats.indexOf({currentSeatId: currentSeatId, }),1);
                        if (!leftSeatIsCurrentlyReserved && !leftSeatIsTaken && leftSeatProperties) {
                            markSeatAsBlue(leftSeatProperties.id);                
                        }
                        if (!rightSeatIsCurrentlyReserved && !rightSeatIsTaken && rightSeatProperties) {
                            markSeatAsBlue(rightSeatProperties.id);
                        }
        
                        if (leftSeatIsCurrentlyReserved || rightSeatIsCurrentlyReserved) {
                            
                            markSeatAsGreenish(currentSeatId);
                            
                        }
                    }
                    
                   
                }
                setSeat((prev)=>({
                ...prev,
                currentReservationSeats,
                }));
                
            }}
            className={
                seatTaken
                ? "seat seat-taken"
                : currentlyReserved
                ? "seat seat-current-reservation"
                : "seat"
            }
            value={currentSeatId}
            key={`row${row}-seat${i}`}
            >
            <FontAwesomeIcon className="fa-2x couch-icon " icon={faCouch} />
            </button>
          </td>
            
        );
        if (seatIndex < maxIndex) {
            seatIndex += 1;
        }
        }
    }

    return renderedSeats;
    };

  
    return( 
        <Container >
            <Row className="justify-content-center ">
              <Button 
              type="primary" 
              className="mb-3" 
              disabled={seat.currentReservationSeats.length===0?true:false} 
              onClick={(e) => tryReservation(e)}>
              Confirm<FontAwesomeIcon className="text-white mr-1 fa-1x btn-payment__icon" icon={faShoppingCart}/>
              </Button>
          
            </Row>
            <Row className="justify-content-center ">
            <table className="">
                    <tbody>{renderRows(seat.maxRow,seat.maxNumberOfRow)}</tbody>
            </table>
            </Row>
            <Row className="justify-content-center my-2">
            <div className="text-center text-white font-weight-bold cinema-screen">
            CINEMA SCREEN
            </div>
            </Row>
            <PopUpModel show={state.modalShow} setState={setState} history={history}/>
            
           
        </Container>
    );
};
export default ShowAuditorium;