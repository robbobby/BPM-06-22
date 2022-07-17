import Style from './Header.module.scss';
import { ThemeButton } from "../ThemeButton/ThemeButton";
import { LogOut } from "../../App/AuthProvider";
import { useNavigate } from "react-router-dom";
import Button, { ButtonSize, ButtonVariant } from "../Button/Button";

export function Header() {
  const navigate = useNavigate();
  function signOut() {
    LogOut();
    navigate("/login");
  }
  
  return (
    <div className={Style.header}>
      <div className={Style.logo}>
          Levity
      </div>*
      <ThemeButton/>
      <nav className={Style.nav}>
        <ul >
          <li>
            <Button variant={ButtonVariant.Link} size={ButtonSize.Large} style={{minWidth: "auto"}} onClick={() => navigate("/about")}>About</Button>
          </li>
          
          <li>
            <Button variant={ButtonVariant.Link} size={ButtonSize.Large} style={{minWidth: "auto"}} onClick={() => navigate("/contact")}>Contact</Button>
            
          </li>
          <li>
          {!localStorage.getItem('user') ?
              <Button variant={ButtonVariant.Link} style={{minWidth: "auto"}} onClick={() => navigate("/login")}>Login</Button>
            :
              <Button variant={ButtonVariant.Link} size={ButtonSize.Large} style={{minWidth: "auto"}} onClick={() => signOut()}>Sign out</Button>
          }
          </li>
        </ul>
      </nav>
    </div>
  );
}
