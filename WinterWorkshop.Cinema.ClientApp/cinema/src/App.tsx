import React from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-notifications/lib/notifications.css';
import 'react-bootstrap-typeahead/css/Typeahead.css';
import { Route, Switch, Redirect } from 'react-router-dom';
import { NotificationContainer } from 'react-notifications';
// components
import Header from './components/Header';

import Projections from './components/ProjectionComponent/Projections'
import Dashboard from './components/admin/Dashboard';
import UserProfile from "../src/components/user/UserProfile";
import UserSingUp from '../src/components/user/UserSingUp';



function App() {
  return (
    <React.Fragment>
      <Header/>
      <div className="set-overflow-y">
      <Switch>
        <Redirect exact from="/" to="dashboard/Projections" />
        <Route path="/Projections" component={Projections} />
        <Route path="/dashboard" component={Dashboard} />
        <Route path="/userprofile" component={UserProfile} />
        <Route path="/newUser" component={UserSingUp} />
      </Switch>
      <NotificationContainer />
      </div>
    </React.Fragment>
  );
}

export default App;
