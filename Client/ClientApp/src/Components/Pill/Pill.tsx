import Style from "./pill.module.scss";
import React from "react";

interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  size: PillSizes;
  color: PillColours;
  text?: string;
  children?: React.ReactNode;
  asButton?: boolean;
}

export function Pill(props: Props) {
  const style = [
    Style.base,
    props.color ? Style[props.color] : "",
    props.size ? Style[props.size] : "",
    props.asButton ? Style.asButton : "",
  ]

  if (props.asButton) {
    return (
      <button className={style.join(" ")} {...props}>
        {props.text}
        {props.children}
      </button>
    );
  }
  
  return (
    <span className={style.join(" ")}>
      {props.text}
      {props.children}
    </span>
  );
}

export enum PillSizes {
  Small = "small",
  Medium = "medium",
  Large = "large"
};

export enum PillColours {
  GreyDark = "darkgrey",
  GreyMid = "midgrey",
  GreyLight = "lightgrey",
  Black = "black",
  Blue = "blue",
  BlueMid = "midblue",
  BlueDark = "darkblue",
  Green = "green",
  GreenMid = "midgreen",
  GreenDark = "darkgreen",
  Orange = "orange",
  OrangeLight = "lightorange",
  Purple = "purple",
  PurpleDark = "darkpurple",
  Pink = "pink",
};
