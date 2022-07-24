import React from "react";
import Style from "./Card.module.scss";

export function Card(props: Props) {
  
  const style = [
    Style.base,
    props.shadow ? Style.boxShadow : "",
    props.center ? Style.center : "",
    props.noPadding ? Style.noPadding : "",
    Style.restrictWidth,
    props.borderColour ? Style[`border${props.borderColour}`] : "",
  ]
  
  
  return (
    <section className={style.join(" ")}>
      {/*{props.logo ? <img className={Style.logo} src={contxtLogo} /> : null}*/}
      {props.title && (
        <header className={Style.header}>
          <h1 className={Style.title}>{props.title}</h1>
        </header>
      )}

      {props.children}
      {/*{props.loading ? <Loader /> : props.children}*/}
    </section>
  );
}

interface Props {
  center?: boolean;
  title?: string,
  borderColour?: CardBorderColour,
  logo?: boolean,
  shadow?: boolean,
  loading?: boolean,
  noPadding?: boolean,
  restrictWidth?: boolean,
  children?: React.ReactNode
};

export enum CardBorderColour {
  Purple = "Purple",
  DarkPurple = "DarkPurple",
  Orange = "Orange",
  LightOrange = "LightOrange",
  Green = "Green",
  MidGreen = "MidGreen",
  DarkGreen = "DarkGreen",
  Blue = "Blue",
  MidBlue = "MidBlue",
  DarkBlue = "DarkBlue",
}