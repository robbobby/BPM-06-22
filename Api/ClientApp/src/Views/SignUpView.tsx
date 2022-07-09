import axios from "axios";
import { FormEvent, SyntheticEvent, useState } from 'react';

interface Props {
  setLoginView: (loginView: boolean) => void;
}


interface SignUpRequest {
  email: string;
  password: string;
}

export function SignUpView(props: Props) {
  const handleSignUpRequest = (event: FormEvent<HTMLFormElement>) => {
    console.log(event.currentTarget.email.value);
    console.log(event.currentTarget.password.value);

    event?.preventDefault();
    axios.post(`${process.env.REACT_APP_API_URL}/api/User/Register`, {
      Username: event.currentTarget.email.value,
      Password: event.currentTarget.password.value,
      FirstName: event.currentTarget.email.value,
      LastName: event.currentTarget.email.value,
      EmailAddress: event.currentTarget.email.value
    }).then(res => {
        console.log(res);
      }
    ).catch(err => {
      console.log(err);
    });
  }
  return (
    <div>
      <h1>Sign Up</h1>
      <form onSubmit={handleSignUpRequest}>
        <div>
          <label>Email</label>
          <input type="text" name="email"/>
        </div>
        <div>
          <label>Password</label>
          <input type="password" name="password"
          />
        </div>
        <button type="submit">Sign Up</button>
        <button onClick={() => props.setLoginView(true)}>Login</button>
      </form>
    </div>
  );
}
