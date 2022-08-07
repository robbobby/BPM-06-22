import React from "react";
import Style from './Accordion.module.scss';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FaIcon, FaIconSize } from "../../Utils/fontAwesomeIcons";
import { Pill, PillColours, PillSizes } from "../Pill/Pill";

export function Accordion(props: Props) {
  const [isOpen, setIsOpen] = React.useState(false);

  return (
    <>
      <div className={Style.accordionContainer}>
      <div className={Style.tabs}>
        <span className={Style.innerTab}>
        <FontAwesomeIcon className={Style.icon} size={FaIconSize.SizeL} icon={isOpen ? FaIcon.AngleDown : FaIcon.AngleRight} onClick={() => setIsOpen(!isOpen)}/>
        <Pill size={PillSizes.Medium} color={PillColours.Orange}>{props.header}</Pill>
        </span>
      </div>
        {isOpen &&
            <div className={`${Style.tabs} ${Style.openBody}`}>
                <p className={Style.tab}>{props.body}</p>
            </div>
        }
      </div>
    </>
  );
}

interface Props {
  children?: React.ReactNode;
  header: string;
  body: string;
}
