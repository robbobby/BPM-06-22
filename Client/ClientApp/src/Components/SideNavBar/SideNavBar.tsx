import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Style from './SideBarNav.module.scss'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import '../../utils/fontAwesomeIcons'
import { FaIcon, FaIconSize } from "../../utils/fontAwesomeIcons";

export function SideNavBar(props: Props) {
  const showSidebar = () => props.setSidebar(!props.sidebar);
  const [selectedPage, setSelectedPage] = useState('');

  useEffect(() => {
    setSelectedPage(window.location.pathname);
  });

  const sideBarNavItems = [

    {url: "Dashboard", icon : FaIcon.House},
    {url: "Account", icon : FaIcon.User},
    {url: "Settings", icon: FaIcon.Gear},
    {url: "Backlog", icon: FaIcon.TableList},
    {url: "Todo", icon: FaIcon.ListCheck},
    // {url: "Logout", icon: FaIcon.House},
  ]

  return (
    <nav className={props.sidebar ? `${Style.sidebar} ${Style.active}` : Style.sidebar}>
      <button className={Style.hamburger} type="button" onClick={showSidebar}/>
      <ul>

        {sideBarNavItems.map((item, index) => {
          return (
            <li className={selectedPage === `/${item.url.toLowerCase()}` ? Style.selected : ""}>
              <Link to={`/${item.url.toLowerCase()}`}>
                <div>
                  <FontAwesomeIcon className={Style.iconContainer} icon={item.icon} size={FaIconSize.S1}/>
                  <span>{item.url}</span>
                </div>
              </Link>
            </li>
          )
        })}
      </ul>
    </nav>
  );
}

interface Props {
  sidebar: boolean;
  setSidebar: (sidebar: boolean) => void;
}
