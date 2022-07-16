/***
 *
 *   CARD
 *   Universal container for grouping UI components together
 *
 *   PROPS
 *   title: title string (optional)
 *   loading: boolean to toggle the loading animation (optional)
 *   shadow: apply a box shadow
 *   center: center align the card
 *   noPadding: remove the padding
 *   restrictWidth: restrict the width of the card on large screens
 *
 **********/

import React from "react";
import Style from "./Card.module.scss";
import PropTypes from "prop-types";

export function Card(props: any) {
  const cardStyle = Style.base + Style.borderBlue;

  return (
    <section className={`${Style.base} ${Style.borderPurple}`}>
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

Card.propTypes = {
  border: PropTypes.string,
  logo: PropTypes.bool,
  shadow: PropTypes.string,
  loading: PropTypes.bool,
  noPadding: PropTypes.bool,
  restrictWidth: PropTypes.bool,
};