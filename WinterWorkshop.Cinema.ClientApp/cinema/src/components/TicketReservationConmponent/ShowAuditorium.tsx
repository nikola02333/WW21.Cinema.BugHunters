import React,{memo,useEffect, useState} from 'react'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faShoppingCart, faCouch } from "@fortawesome/free-solid-svg-icons";
import { Container, Row, Col } from "react-bootstrap";
import "./../../index.css";
import { serviceConfig } from "../../appSettings";
import { NotificationManager } from "react-notifications";
import { Button, Modal } from 'antd';
import 'antd/dist/antd.css';
import { useHistory } from "react-router-dom";


const ShowAuditorium = memo((props:{seats,setSeat,tryReservation}) => {
    console.log("ChooseSeats");
    
    const [state,setState]=useState({
      loading: false,
      visible: false,
      separately:false
    });

    const history = useHistory();

    const showModal = () => {
      setState((prev)=>({
        ...prev,
        visible: true
      }));
    };
  
    const handleCancel = () => {
      setState((prev)=>({...prev, visible: false }));
    };
    
    let allButtons: any;

    const renderRows = (rows: number, seats: number) => {
    
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
        return rowsRendered;
      };

    const checkIfSeatIsTaken = (currentSeatId: string) => {
        return props.seats.reservedSeats.some(seat=>seat.id===currentSeatId);
      };
    
      const checkIfSeatIsCurrentlyReserved = (currentSeatId) => {
        return props.seats.currentReservationSeats.some(reserved=>reserved.currentSeatId === currentSeatId);
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
            console.log("Mark BLUE");
            console.log(allButtons[i].value);
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

    if (props.seats.seats.length > 0) {
        for (let i = 0; i < seats; i++) {
          
          let currentSeatId = props.seats.seats[seatIndex].id;
          let currentlyReserved = props.seats.currentReservationSeats.some((seat) => seat.currentSeatId === currentSeatId);
          let seatTaken = props.seats.reservedSeats.some((seat) => seat.id === currentSeatId);

        renderedSeats.push(
          <td key={`row${row}-seat${i}`}>
            <button
            onClick={(e) => {
              console.log("KLIK "+`row${row}-seat${i} `+currentSeatId);
                let currentRow = row;
                let currentNumber = i;
                let currSeatId = currentSeatId;

                let leftSeatIsCurrentlyReserved = false;
                let leftSeatIsTaken = false;
                let rightSeatIsCurrentlyReserved = false;
                let rightSeatIsTaken = false;
                let leftSeatProperties = getSeatByPosition(currentRow + 1,currentNumber);
                let rightSeatProperties = getSeatByPosition(currentRow + 1, currentNumber + 2);
                let currentReservationSeats = props.seats.currentReservationSeats;

                if (leftSeatProperties) {
                    leftSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(leftSeatProperties.id);
                    
                    leftSeatIsTaken = checkIfSeatIsTaken(leftSeatProperties.id);
                }
                console.log("K");
                if (rightSeatProperties) {
                    rightSeatIsCurrentlyReserved = checkIfSeatIsCurrentlyReserved(rightSeatProperties.id);
                    console.log(rightSeatIsCurrentlyReserved);
                    rightSeatIsTaken = checkIfSeatIsTaken(rightSeatProperties.id);
                }

                if (!checkIfSeatIsCurrentlyReserved(currSeatId)) {
                    if (props.seats.currentReservationSeats.length !== 0 && 
                        getButtonBySeatId(currentSeatId).className !=="seat nice-green-color" && !state.separately) {
                          showModal();
                        return;
                    }

                    if (!leftSeatIsCurrentlyReserved && !leftSeatIsTaken && leftSeatProperties) {
                    markSeatAsGreenish(leftSeatProperties.id);
                    }

                    if (!rightSeatIsCurrentlyReserved && !rightSeatIsTaken && rightSeatProperties) {
                    markSeatAsGreenish(rightSeatProperties.id);
                    }

                    if (props.seats.currentReservationSeats.includes({currentSeatId}) === false) {
                      console.log("PUSH")
                    currentReservationSeats.push({
                         currentSeatId,
                    });
                    console.log(currentReservationSeats);
                    }
                } else {
                    if (leftSeatIsCurrentlyReserved && rightSeatIsCurrentlyReserved ) {
                         console.log("000");
                        markWholeRowSeatsAsBlue();
                        currentReservationSeats = [];
                    } else {
                        currentReservationSeats.splice(currentReservationSeats.indexOf({currentSeatId: currentSeatId, }),1);
                        console.log("SLICE");
                        if (!leftSeatIsCurrentlyReserved && !leftSeatIsTaken && leftSeatProperties) {
                            markSeatAsBlue(leftSeatProperties.id);
                            console.log("111");
                            console.log(leftSeatProperties.id);
                            
                        }
                        if (!rightSeatIsCurrentlyReserved && !rightSeatIsTaken && rightSeatProperties) {
                            markSeatAsBlue(rightSeatProperties.id);
                            console.log("222");
                        }
        
                        if (leftSeatIsCurrentlyReserved || rightSeatIsCurrentlyReserved) {
                          
                          console.log("333");
                          console.log(currentSeatId);
                            setTimeout(() => {
                            markSeatAsGreenish(currentSeatId);
                            }, 150);
                        }
                    }

                    console.log("KRAJ KLIKAAA");
                    props.setSeat((prev)=>({
                    ...prev,
                    currentReservationSeats,
                    }));
                }
                console.log("KRAJ KLIKA");
                props.setSeat((prev)=>({
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
    

    const { visible, loading } = state;
    return( 
        <Container >
            <Row className="justify-content-center ">
              <Button type="primary" shape="round" className="mb-3" onClick={(e) => props.tryReservation(e)}>
              Confirm<FontAwesomeIcon className="text-white mr-1 fa-1x btn-payment__icon" icon={faShoppingCart}/>
              </Button>
            </Row>
            <Row className="justify-content-center ">
            <table className="">
                    <tbody>{renderRows(props.seats.maxRow,props.seats.maxNumberOfRow)}</tbody>
            </table>
            </Row>
            <Row className="justify-content-center my-2">
            <div className="text-center text-white font-weight-bold cinema-screen">
            CINEMA SCREEN
            </div>
            </Row>
            <Modal
              visible={visible}
              title="Information"
              onCancel={handleCancel}
              footer={[
                <Button key="back" onClick={handleCancel}>
                  Return
                </Button>,
                <Button key="separately" loading={loading} 
                onClick={()=>{setState((prev)=>({...prev,separately:true})); handleCancel();}}>
                  Buy separately
                </Button>,
                <Button key="projection" type="primary" loading={loading} onClick={()=>{handleCancel(); history.push('/dashboard/Projections');}}>
                  Projection
                </Button>
              ]}
            >
              <p>You can buy tickets separately or change projection time.</p>
              
        </Modal>
           
        </Container>
    );
});
export default ShowAuditorium;