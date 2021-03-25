import React, { SetStateAction, Dispatch } from 'react'
import { Button , Modal} from "react-bootstrap";
import { History } from 'history';
import {IStateShowAuditoriums} from "./ShowAuditorium";

interface IProps{
    show:boolean,
    setState: Dispatch<SetStateAction<IStateShowAuditoriums>>
    history : History
}

 const PopUpModel: React.FC<IProps> =({show,setState,history})=>{
    return(
        <Modal
        show={show}
        onHide={() => setState((prev)=>({...prev, modalShow:false}))}
        dialogClassName="modal-40w"
        aria-labelledby="example-custom-modal-styling-title"
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title id="contained-modal-title-vcenter">
            Informatin
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>You can buy tickets separately or change projection time.</p>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="outline-secondary" onClick={() => setState((prev)=>({...prev, modalShow:false}))}>Close</Button>
          <Button key="separately" variant="outline-primary"
          onClick={()=>{setState((prev)=>({...prev,separately:true})); setState((prev)=>({...prev, modalShow:false}));}}>
            Buy separately
        </Button>
        <Button key="projection" variant="outline-primary" onClick={()=>{setState((prev)=>({...prev, modalShow:false})); history.push('/dashboard/Projections');}}>
          Projection
        </Button>
        </Modal.Footer>
      </Modal>
    )
}
export default PopUpModel;