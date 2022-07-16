import Style from './Header.module.scss';
import { ThemeButton } from "../ThemeButton/ThemeButton";

export function Header() {
  return (
    <div className={Style.header}>
      <div className={Style.logo}>
          Robs new Gravity
      </div>
      <ThemeButton/>
      <nav className={Style.nav}>
        <ul >
          <li>
            <a href="login">Signin</a>
          </li>
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
