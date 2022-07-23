import Style from "./pill.module.scss";
import PropTypes from "prop-types";

export function Pill(props: Props) {

  const style = [
    Style.base,
    props.color ? Style[props.color] : "",
    props.size ? Style[props.size] : "",
  ]

  return (
    <span className={style.join(" ")}>
      {props.text}
      {props.children}
    </span>
  );
}

type Props = {
  size: PillSizes,
  color: PillColours,
  text?: string
  children?: React.ReactNode
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
