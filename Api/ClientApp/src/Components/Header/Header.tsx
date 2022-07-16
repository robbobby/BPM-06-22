import Style from './Header.module.scss';

export function Header() {
  // return header with a nav bar and a logo
  return (
    <div className={Style.header}>
      <div className={Style.logo}>
          Robs new Gravity
      </div>
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