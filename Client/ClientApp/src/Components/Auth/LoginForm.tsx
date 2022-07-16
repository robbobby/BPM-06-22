import axios from "axios";
import { SetToken, TokenResponse } from "../../App/AuthProvider";
import { Card } from "../Card/Card";
import { useNavigate } from "react-router-dom";
import { FormEvent } from "react";
import { ApiReq } from "../Hooks/ApiReq";
import {AxiosResponse} from "axios";

interface Props {
  setLoginView: (loginView: boolean) => void;
}

export function LoginForm(props: Props) {
  const navigate = useNavigate();
  
  const handleLoginRequest = (event: FormEvent<HTMLFormElement>) => {
    event?.preventDefault();
    ApiReq("/api/auth/login", "post", {
      emailAddress: event.currentTarget.email.value,
      password: event.currentTarget.password.value
    }).then((res: AxiosResponse<TokenResponse> ) => {
      SetToken(res.data);
      navigate('/dashboard');
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