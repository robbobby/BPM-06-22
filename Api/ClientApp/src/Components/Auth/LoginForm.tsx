import axios from "axios";
import React from 'react';
import jwtDecode from 'jwt-decode';
import { SetToken, TokenResponse } from "../../App/AuthProvider";
import { Card } from "../Card/Card";

interface UserLoginResponseBody {
  accessToken?: string;
  refreshToken?: string;
  accessTokenExpiration?: number;
  role: string;
}

interface Props {
  setLoginView: (loginView: boolean) => void;
}


interface TokenValues {
  role: string,
  exp: number
}

export function LoginForm(props: Props) {
  console.log(localStorage.getItem('user'));
  const handleLoginRequest = (event: React.FormEvent<HTMLFormElement>) => {
    event?.preventDefault();
    axios.post(`${process.env.REACT_APP_API_URL}/api/Auth/Login`, {
      emailAddress: event.currentTarget.email.value,
      password: event.currentTarget.password.value
    }).then((res: { data: TokenResponse }) => {
      SetToken(res.data);
    })
      .catch(err => {
        console.log(err);
      });
  }
  return (
    <div>
      <h1>Login</h1>
      <Card>
        <div>
      <form onSubmit={handleLoginRequest}>
        <div>
          <label>Email</label>
          <input type="text" name="email"/>
        </div>
        <div>
          <label>Password</label>
          <input type="password" name="password"/>
        </div>
        <button type="submit">Login</button>
        <button onClick={() => props.setLoginView(false)}>Sign Up</button>
      </form>
        </div>
      </Card>
    </div>
  );
}