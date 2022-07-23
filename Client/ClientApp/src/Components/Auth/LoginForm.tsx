import { AxiosResponse } from "axios";
import { SetToken, Token } from "../../App/AuthProvider";
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
      <Card shadow={true} borderColour={CardBorderColour.LightOrange} restrictWidth={true} center={true}>
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
            <span className={Style.buttonContainer}>
              <Button size={ButtonSize.Small} variant={ButtonVariant.Primary} type="submit">Sign in</Button>
              <Button size={ButtonSize.Small} onClick={() => props.setLoginView(false)}>Back to login</Button>
            </span>
          </form>
        </div>
      </Card>
    </div>
  );
}