import { useState } from "react";
import { Link } from "react-router-dom";
import Style from './SideBarNav.module.scss'

export function SideNavBar(props: any) {
  const [sidebar, setSidebar] = useState(false);
  const showSidebar = () => setSidebar(!sidebar);
  return (
    <nav className={sidebar ? `${Style.sidebar} ${Style.active}` : Style.sidebar}>
      <button className={Style.hamburger} type="button" onClick={showSidebar}>
      </button>
      <ul onClick={showSidebar}>
        <li><Link to="/">Home</Link></li>
        <li><Link to="/services">Services</Link></li>
        <li><Link to="/contact">Contact</Link></li>
      </ul>
    </nav>
  );
  
}