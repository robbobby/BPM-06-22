import React, { Dispatch, SetStateAction, useEffect } from "react";
import Style from "./Modal.module.scss";
import Button from "../Button/Button";

interface Props {
  children?: React.ReactNode;
  show: boolean;
  setShow: Dispatch<SetStateAction<boolean>>;
  onCancel?: () => void;
}

export function Modal(props: Props) {

  const show = props.show;
  if (!props.show) {
    return null
  }
  return (
      <div className={Style.modal}>
        <div className={Style.modalBackground} onClick={props.onCancel}></div>
        <div className={Style.modalContent}>
          {props.children}
        </div>
      </div>
  );
}