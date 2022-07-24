import Style from './Header.module.scss';
import { ThemeButton } from "../ThemeButton/ThemeButton";
import { LogOut } from "../../App/AuthProvider";
import { useNavigate } from "react-router-dom";
import Button, { ButtonSize, ButtonVariant } from "../Button/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FaIcon, FaIconSize } from "../../utils/fontAwesomeIcons";

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
      <nav className={Style.nav}>
      </nav>
    </div>
  );
}
