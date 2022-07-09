import React from 'react';
import logo from './logo.svg';
import './App.css';
import { LoginView } from "./Views/LoginView";
import { SignUpView } from "./Views/SignUpView";
import axios from 'axios';

function App() {
  // switch between login and signup view
  const [loginView, setLoginView] = React.useState(true);
  const [view, setView] = React.useState("login");

  const handleLoginSubmit = (username: string, password: string) => {
    axios.post(`${process.env.SERVER}/api/login`, {
      username: username,
      password: password
    }).then(res => {
        console.log(res);
      }
    ).catch(err => {
      console.log(err);
    });
  }


  return (
    <div className="App">
      <header className="App-header">
        {loginView ?
          <LoginView setLoginView={setLoginView} />
          :
          <SignUpView setLoginView={setLoginView} />
        }
      </header>
    </div>
  );
}

export default App;
