import { MouseEventHandler, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Style from './SideBarNav.module.scss'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import '../../Utils/fontAwesomeIcons'
import { FaIcon, FaIconSize } from "../../Utils/fontAwesomeIcons";
import AuthProvider, { LogOut } from "../../App/AuthProvider";
import { ThemeButton } from "../ThemeButton/ThemeButton";
import useDarkMode from "use-dark-mode";

interface NavItems {
  icon: FaIcon;
  url: string
  action?: MouseEventHandler
}

export function SideNavBar(props: Props) {
  const showSidebar = () => props.setSidebar(!props.sidebar);
  const [selectedPage, setSelectedPage] = useState('');

  const darkMode = useDarkMode(true);

  useEffect(() => {
    setSelectedPage(window.location.pathname);
  });

  const topNavItems = [
    {url: "Dashboard", icon: FaIcon.House},
    {url: "Account", icon: FaIcon.User},
    {url: "Backlog", icon: FaIcon.TableList},
    {url: "Todo", icon: FaIcon.ListCheck},
  ]

  const bottomNavItems = [
    {url: "Settings", icon: FaIcon.Gear},
    {url: "Logout", icon: FaIcon.Logout, action: LogOut}
  ]

  function setNavItems(navItems: NavItems[]) {
    return navItems.map((item: NavItems) =>
      <Link onClick={item.action} to={`/${item.url.toLowerCase()}`} className={selectedPage === `/${item.url.toLowerCase()}` ? `${Style.selected + " " + Style.Link}` : `${Style.Link}`}>
        <li >
          <FontAwesomeIcon className={Style.iconContainer} icon={item.icon} size={FaIconSize.S1}/>
          <span>{item.url}</span>
        </li>
      </Link>
    )
  }

  return (
    <nav className={props.sidebar ? Style.sidebar : `${Style.sidebar} ${Style.collapsed}`}>
      <button className={Style.hamburger} type="button" onClick={showSidebar}/>
      <div>
        <ul style={{padding: "0", margin: "0"}}>
          {setNavItems(topNavItems)}
        </ul>
      </div>

      <div className={Style.bottomNavItems}>
        <ul>
          <li>
            <div onClick={darkMode.toggle} className={Style.themeButton}>
              <div className={Style.iconContainer} style={{width: "30px", paddingLeft: "4px", marginRight: "15px"}}>
                <ThemeButton/>
              </div>
              <span className={Style.themeSpan}>Theme</span>
            </div>
          </li>
          {setNavItems(bottomNavItems)}
        </ul>
      </div>
    </nav>
  );
}

interface Props {
  sidebar: boolean;
  setSidebar: (sidebar: boolean) => void;
}
