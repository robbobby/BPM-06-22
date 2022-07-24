import axios from "axios";
import { FormEvent } from 'react';
import Button, { ButtonSize } from "../Button/Button";
import { Card, CardBorderColour } from "../Card/Card";
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
    <div className={Style.container}>
      <h1>Sign Up</h1>
      <Card shadow={true} borderColour={CardBorderColour.LightOrange} restrictWidth={true} center={true}>
        <form onSubmit={handleSignUpRequest}>
          <div style={{marginRight: "13px"}}>
            <label>Email</label>
            <input type="text" name="email"/>
          </div>
          <div style={{marginRight: "13px"}}>
            <label>Password</label>
            <input type="password" name="password"
            />
          </div>
          <span className={Style.buttonContainer}>
            <Button size={ButtonSize.Small} type="submit">Sign up</Button>
            <Button size={ButtonSize.Small} onClick={() => props.setLoginView(true)}> Back to Login</Button>
          </span>
        </form>
      </Card>
    </div>
  );
}
