import axios from 'axios';

import {serviceConfig}  from './appSettings';

export default axios.create({
  baseURL: serviceConfig.baseURL,
  headers: {
    "Content-Type": "application/json",
    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
  }
});