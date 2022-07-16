import Style from './Header.module.scss';
import { ThemeButton } from "../ThemeButton/ThemeButton";
import AuthProvider, { LogOut } from "../../App/AuthProvider";
import { useNavigate } from "react-router-dom";

export function Header() {
  const navigate = useNavigate();
  function signOut() {
    LogOut();
    navigate("/login");
  }
  
  return (
    <div className={Style.header}>
      <div className={Style.logo}>
          Robs new Gravity
      </div>
      <ThemeButton/>
      <nav className={Style.nav}>
        <ul >
          {!localStorage.getItem('user') ?
            <li>
              <a href="login">Signin</a>
            </li>
            : 
            <li>
              <button onClick={() => signOut()}>Signout</button>
            </li>
          }
          <li>
            <a href="about">About</a>
          </li>
          <li>
            <a href="contact">Contact</a>
          </li>
        </ul>
      </nav>
    </div>
  );
}
