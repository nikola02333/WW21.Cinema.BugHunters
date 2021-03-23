import React,{memo,useEffect} from 'react'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faShoppingCart, faCouch } from "@fortawesome/free-solid-svg-icons";
import { Container, Row, Col, Button } from "react-bootstrap";
import "./../../index.css";

const ShowAuditorium = memo((props:{seats,setSeat}) => {
    console.log("ChooseSeats");
    let allButtons: any;
    // var auditorium;
    useEffect(()=>{
        console.log("STATE")
        console.log(props.seats);
    },[props.seats]);

    const renderRows = (rows: number, seats: number) => {
        console.log("1renderRows  ====" +props.seats.seats.length);
        const rowsRendered: JSX.Element[] = [];
        if (props.seats.seats.length > 0) {
          for (let i = 0; i < rows; i++) {
            const startingIndex = i * seats;
            const maxIndex = (i + 1) * seats;
    
            rowsRendered.push(
              <tr key={i}>{renderSeats(seats, i, startingIndex, maxIndex)}</tr>
            );
          }
        }
        console.log("DUZINA "+rowsRendered.length);
        return rowsRendered;
      };

    const checkIfSeatIsTaken = (currentSeatId: string) => {
        for (let i = 0; i < props.seats.reservedSeats.length; i++) {
          if (props.seats.reservedSeats[i].seatId === currentSeatId) {
            return true;
          }
        }
        return false;
      };
    
      const checkIfSeatIsCurrentlyReserved = (currentSeatId) => {
        return props.seats.currentReservationSeats.includes(currentSeatId);
      };
    
      const getSeatByPosition = (row: number, number: number) => {
        for (let i = 0; i < props.seats.seats.length; i++) {
          if (props.seats.seats[i].number === number && props.seats.seats[i].row === row) {
            return props.seats.seats[i];
          }
        }
      };
    
      const getSeatById = (seatId: string) => {
        for (let i = 0; i < props.seats.seats.length; i++) {
          if (props.seats.seats[i].id === seatId) {
            return props.seats.seats[i];
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
    console.log("1POCETAK")
    if (props.seats.seats.length > 0) {
        for (let i = 0; i < seats; i++) {
        let currentSeatId = props.seats.seats[seatIndex].id;
        console.log(currentSeatId);
        let currentlyReserved =
            props.seats.currentReservationSeats.filter(
            (seat) => seat.currentSeatId === currentSeatId
            ).length > 0;
        let seatTaken =
            props.seats.reservedSeats.filter((seat) => seat.seatId === currentSeatId)
            .length > 0;

        renderedSeats.push(
            <button
            onClick={(e) => {
                let currentRow = row;
                let currentNumber = i;
                let currSeatId = currentSeatId;

                let leftSeatIsCurrentlyReserved = false;
                let leftSeatIsTaken = false;
                let rightSeatIsCurrentlyReserved = false;
                let rightSeatIsTaken = false;
                let leftSeatProperties = getSeatByPosition(
                currentRow + 1,
                currentNumber
                );
                let rightSeatProperties = getSeatByPosition(currentRow + 1, currentNumber + 2);
                let currentReservationSeats = props.seats.currentReservationSeats;

                if (leftSeatProperties) {
                    leftSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(leftSeatProperties.id);

                    leftSeatIsTaken = checkIfSeatIsTaken(leftSeatProperties.id);
                }

                if (rightSeatProperties) {
                    rightSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(rightSeatProperties.id);
                    rightSeatIsTaken = checkIfSeatIsTaken(rightSeatProperties.id);
                }

                if (!checkIfSeatIsCurrentlyReserved(currSeatId)) {
                    if (props.seats.currentReservationSeats.length !== 0 && 
                        getButtonBySeatId(currentSeatId).className !=="seat nice-green-color") {
                        return;
                    }

                    if (!leftSeatIsCurrentlyReserved && !leftSeatIsTaken && leftSeatProperties) {
                    markSeatAsGreenish(leftSeatProperties.id);
                    }

                    if (!rightSeatIsCurrentlyReserved && !rightSeatIsTaken && rightSeatProperties) {
                    markSeatAsGreenish(rightSeatProperties.id);
                    }

                    if (props.seats.currentReservationSeats.includes({currentSeatId: currentSeatId,}) === false) {
                    currentReservationSeats.push({
                        currentSeatId: currentSeatId,
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
                            setTimeout(() => {
                            markSeatAsGreenish(currentSeatId);
                            }, 150);
                        }
                    }
                    props.setSeat((prev)=>({
                    ...prev,
                    currentReservationSeats: currentReservationSeats,
                    }));
                }
                props.setSeat((prev)=>({
                ...prev,
                currentReservationSeats: currentReservationSeats,
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
            <FontAwesomeIcon className="fa-2x couch-icon" icon={faCouch} />
            </button>
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
                <Button /*onClick={(e) => tryReservation(e)}*/ className="btn-payment">
                Confirm<FontAwesomeIcon className="text-white mr-1 fa-1x btn-payment__icon" icon={faShoppingCart}/>
                </Button>
            </Row>
            <Row className="justify-content-center ">
            <table className="table-cinema-auditorium">
                    <tbody>{renderRows(props.seats.maxNumberOfRow, props.seats.maxRow)}</tbody>
            </table>
            </Row>
            <Row className="justify-content-center my-2">
            <div className="text-center text-white font-weight-bold cinema-screen">
            CINEMA SCREEN
            </div>
            </Row>
           
        </Container>
    );
});
export default ShowAuditorium;