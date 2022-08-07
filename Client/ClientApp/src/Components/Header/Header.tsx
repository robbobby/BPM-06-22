import Style from './Header.module.scss';
import { ThemeButton } from "../ThemeButton/ThemeButton";
import { LogOut } from "../../App/AuthProvider";
import { useNavigate } from "react-router-dom";
import Button, { ButtonSize, ButtonVariant } from "../Button/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FaIcon, FaIconSize } from "../../Utils/fontAwesomeIcons";
import { AccountSelector } from "../AccountSelector/AccountSelector";

export function Header() {
  const navigate = useNavigate();

  function signOut() {
    LogOut();
    navigate("/login");
  }

  return (
    <div className={Style.header}>
      <div className={Style.logo}>
        <FontAwesomeIcon icon={FaIcon.Blog} size={FaIconSize.S3} style={{margin: "8px"}}/>
      </div>
      <div style={{width: "100%", display: "flex", justifyContent: "flex-end", marginTop: "1em", marginRight: "1em"}}>
        <AccountSelector/>
      </div>
    </div>
  );
}
