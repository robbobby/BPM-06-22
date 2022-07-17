import axios from "axios";
import { FormEvent } from 'react';
import Button, { ButtonSize } from "../Button/Button";
import { Card } from "../Card/Card";
import Style from "./Auth.module.scss";

interface Props {
  setLoginView: (loginView: boolean) => void;
}


interface SignUpRequest {
  email: string;
  password: string;
}

export function SignUpForm(props: Props) {
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
      <Card>
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
          <span className={Style.buttonContainer}>
            <Button size={ButtonSize.Small} type="submit">Sign up</Button>
            <Button size={ButtonSize.Small} onClick={() => props.setLoginView(true)}>Login</Button>
          </span>
        </form>
      </Card>
    </div>
  );
}
