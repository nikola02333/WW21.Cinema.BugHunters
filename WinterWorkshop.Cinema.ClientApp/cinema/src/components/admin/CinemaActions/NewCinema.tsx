import React, { useState,useEffect } from "react";
import { withRouter } from "react-router-dom";
import {
  FormGroup,
  FormControl,
  
  Container,
  Row,
  Col,
  FormText,
} from "react-bootstrap";
import { NotificationManager } from "react-notifications";
import { serviceConfig } from "../../../appSettings";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCouch } from "@fortawesome/free-solid-svg-icons";
import { Form, Input, Button, Space, Select,InputNumber } from 'antd';
import { MinusCircleOutlined, PlusOutlined } from '@ant-design/icons';
import {cinemaService} from './../../Services/cinemaService';
import {ICinemaToCreateModel} from './../../../models/ICinemaToCreateModel';

interface IState {
  createCinema:ICinemaToCreateModel;
  submitted:boolean;
};

const NewCinema: React.FC = (props: any) => {
  const [state, setState] = useState<IState>({
    createCinema:{
      name:"",
      address:"",
      cityName:""
    },
    submitted: false
  });

  const onFinish = values => {
    console.log('Received values of form:', values);
     setState((prev)=>({...prev,submitted:true, createCinema: values}));
  };

  useEffect(()=>{
    if(state.submitted){
      addCinema();
    }
  },[state.submitted])

 
  const addCinema = async() => {
    console.log(state.createCinema);
    await cinemaService.addCinema(state.createCinema);
  };


  return (
    <Container>
      <Row className="d-flex justify-content-center">
        <Col xs={5}>
          <h2 className="form-header">Add New Cinema</h2>
          <Form name="dynamic_form_nest_item" onFinish={onFinish} autoComplete="off">

            <Form.Item name={ 'name'}  rules={[{ required: true, message: 'Missing area' }]}>
                <Input  placeholder="Cinema name" />
            </Form.Item>
            <Form.Item name={'address'} rules={[{ required: true, message: 'Missing area' }]}>
                <Input placeholder="Cinema address" />
            </Form.Item>
            <Form.Item name={'cityName'}  rules={[{ required: true, message: 'Missing area' }]}>
                <Input placeholder="Cinema city name" />
            </Form.Item>

        <Form.List name="createAuditoriumModel">
          {(fields, { add, remove }) => (
            <>
              {fields.map(field => (
                <Space key={field.key} style={{ display: 'flex', marginBottom: 8 }} align="baseline">
                  <Form.Item
                    {...field}
                    name={[field.name, 'auditoriumName']}
                    fieldKey={[field.fieldKey, 'auditoriumName']}
                    rules={[{ required: true, message: 'Missing auditorium name' }]}
                  >
                    <Input placeholder="Auditorium name" />
                  </Form.Item>
                  <Form.Item
                    {...field}
                    name={[field.name, 'seatRows']}
                    fieldKey={[field.fieldKey, 'seatRows']}
                    rules={[{ required: true, message: 'Missing number of rows' }]}
                  >
                    <InputNumber min={1}   placeholder="Rows" />
                  </Form.Item>
                  <Form.Item
                    {...field}
                    name={[field.name, 'numberOfSeats']}
                    fieldKey={[field.fieldKey, 'numberOfSeats']}
                    rules={[{ required: true, message: 'Missing number of seats' }]}
                  >
                    <InputNumber min={1}    placeholder="Seats" />
                  </Form.Item>
                  <MinusCircleOutlined onClick={() => remove(field.name)} />
                </Space>
              ))}
              <Form.Item>
                <Button type="dashed" onClick={() => add()} block icon={<PlusOutlined />}>
                  Add auditorium
                </Button>
              </Form.Item>
            </>
          )}
        </Form.List>
        <Form.Item style={{ textAlign: 'center' }}>
          <Button type="primary" htmlType="submit">
            Create
          </Button>
        </Form.Item>
      </Form>
        </Col>
      </Row>
      <Row className="mt-2">
      </Row>
     
    </Container>
  );
};

export default withRouter(NewCinema);
