import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Style from './SideBarNav.module.scss'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import '../../utils/fontAwesomeIcons'
import { FaIcon, FaIconSize } from "../../utils/fontAwesomeIcons";

export function SideNavBar(props: any) {
  const [sidebar, setSidebar] = useState(false);
  const showSidebar = () => setSidebar(!sidebar);
  const [selectedPage, setSelectedPage] = useState('');

  useEffect(() => {
    setSelectedPage(window.location.pathname);
    console.log(selectedPage);
  });

  return (
    <nav className={sidebar ? `${Style.sidebar} ${Style.active}` : Style.sidebar}>
      <button className={Style.hamburger} type="button" onClick={showSidebar}/>
      <ul>
        <li className={selectedPage === '/dashboard' ? Style.selected : ""}>
          <Link to="/dashboard">
            <div>
              <FontAwesomeIcon className={Style.iconContainer} icon={FaIcon.House} size={FaIconSize.S2}/>
              <span>Dashboard</span>
            </div>
          </Link>
        </li>
        <li className={selectedPage === '/backlog' ? Style.selected : ""}>
          <Link to="/backlog">
            <div>
              <FontAwesomeIcon className={Style.iconContainer} icon={FaIcon.TableList} size={FaIconSize.S2}/>
              <span>
              Backlog
            </span>
            </div>
          </Link>
        </li>
        <li className={selectedPage === '/todo' ? Style.selected : ""}>
          <Link className={Style.linkContainer} to="/todo">
            <div>
              <FontAwesomeIcon className={Style.iconContainer} icon={FaIcon.ListCheck} size={FaIconSize.S2}/>
              <span>Todo</span>
            </div>
          </Link>
        </li>
      </ul>
    </nav>
  );
}
