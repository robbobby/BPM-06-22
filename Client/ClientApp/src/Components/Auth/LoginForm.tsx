import { AxiosResponse } from "axios";
import { SetLocalStorageUserFromToken, Token } from "../../App/AuthProvider";
import { Card, CardBorderColour } from "../Card/Card";
import { useNavigate } from "react-router-dom";
import { FormEvent } from "react";
import { ApiReq } from "../Hooks/ApiReq";
import Button, { ButtonSize, ButtonVariant } from "../Button/Button";
import Style from "./Auth.module.scss";

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
    }).then((res: AxiosResponse<Token>) => {
      SetLocalStorageUserFromToken(res.data);
      console.log("Before navigate");
      navigate('/dashboard');
    })
      .catch(err => {
        console.log(err);
      });
  }

  return (
    <div className={Style.container} >
      <h1>Login</h1>
      <Card shadow={true} borderColour={CardBorderColour.LightOrange} restrictWidth={true} center={true}>
        <div>
          <form onSubmit={handleLoginRequest}>
            <div style={{marginRight: "13px"}}>
              <label>Email</label>
              <input type="text" name="email"/>
            </div>
            <div style={{marginRight: "13px"}}>
              <label>Password</label>
              <input type="password" name="password"/>
            </div>
            <div className={Style.buttonContainer}>
              <Button size={ButtonSize.Small} variant={ButtonVariant.Primary} type="submit">Sign in</Button>
              <Button size={ButtonSize.Small} onClick={() => props.setLoginView(false)}>Create Account</Button>
            </div>
          </form>
        </div>
      </Card>
    </div>
  );
}