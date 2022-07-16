import { SignUpForm } from "../Components/Auth/SignUpForm";
import { LoginForm } from "../Components/Auth/LoginForm";
import React from "react";
import { BrowserRouter, Router } from "react-router-dom";

export function AuthView() {
  const [loginView, setLoginView] = React.useState(true);
  return (
    <div>
      {loginView ?
        <LoginForm setLoginView={setLoginView}/>
        :
        <SignUpForm setLoginView={setLoginView}/>
      }
    </div>
  );
}