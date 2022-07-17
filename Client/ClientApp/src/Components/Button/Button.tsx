import React from "react";
import Style from '../Button/Button.module.scss';

export default function Button(props: ButtonProps) {
  const style = [
    Style.base,
    props.variant !== ButtonVariant.Link && !props.boxShadow ? Style.boxShadow : "",
    props.color ? Style[props.color] : "",
    props.variant ? Style[props.variant] : Style[ButtonVariant.Primary],
    props.size ? Style[props.size] : "",
  ]
  
  return (
    <button className={style.join(" ")} type={props.type} {...props}>
      {props.children}
    </button>
  );
}

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  children: React.ReactNode;
  variant?: ButtonVariant;
  size?: ButtonSize;
  boxShadow?: boolean;
}

export enum ButtonVariant {
  Primary = "primary",
  Secondary = "secondary",
  Success = "success",
  Danger = "danger",
  Warning = "warning",
  Info = "info",
  Link = "link",
  Transparent = "transparent"
}

export enum ButtonSize {
  Small = "small",
  Medium = "medium",
  Large = "large",
  ExtraLarge = "extra-large"
}