import axios from "axios";
import React from 'react';

interface Props {
  setLoginView: (loginView: boolean) => void;
}

export function LoginView(props: Props) {
  const handleLoginRequest = (event: React.FormEvent<HTMLFormElement>) => {
    event?.preventDefault();
    axios.post(`${process.env.REACT_APP_API_URL}/api/Auth/Login`, {
      emailAddress: event.currentTarget.email.value,
      password: event.currentTarget.password.value
    }).then(res => {
        console.log(res)
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