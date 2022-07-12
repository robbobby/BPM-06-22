import axios from "axios";
import React from 'react';
import jwtDecode from 'jwt-decode';

interface UserLoginResponseBody {
  accessToken?: string;
  refreshToken?: string;
  accessTokenExpiration?: number;
  role: string;
}

interface Props {
  setLoginView: (loginView: boolean) => void;
}

interface TokenResponse {
  accessToken: string;
  refreshToken: string;
}

interface TokenValues {
  role: string,
  exp: number
}

export function LoginForm(props: Props) {
  const handleLoginRequest = (event: React.FormEvent<HTMLFormElement>) => {
    event?.preventDefault();
    axios.post(`${process.env.REACT_APP_API_URL}/api/Auth/Login`, {
      emailAddress: event.currentTarget.email.value,
      password: event.currentTarget.password.value
    }).then((res: { data: TokenResponse }) => {
      const jwtValues = jwtDecode<TokenValues>(res.data.accessToken);
      
      const user: UserLoginResponseBody = {
        accessToken: res.data.accessToken,
        refreshToken: res.data.refreshToken,
        accessTokenExpiration: jwtValues.exp,
        role: jwtValues.role
      }

      localStorage.setItem("user", JSON.stringify(user));
      
      return res.data;
      })
      .catch(err => {
        console.log(err);
      });
  }
  return (
    <div>
      <h1>Login</h1>
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
  );
}