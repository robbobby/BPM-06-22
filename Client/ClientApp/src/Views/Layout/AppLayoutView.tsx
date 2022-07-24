import { SideNavBar } from "../../Components/SideNavBar/SideNavBar";
import { Header } from "../../Components/Header/Header";
import { DashboardView } from "../DashboardView";
import Style from './AppLayoutView.module.scss';
import { useTheme } from "../../Components/Hooks/UseTheme";
import { useState } from "react";

export function AppLayoutView(props: Props) {
  useTheme();
  const [sidebarExpanded, setSidebarExpanded] = useState(false);


  return (
    <div style={{height: "100%"}}>
    <Header/>
      <SideNavBar sidebar={sidebarExpanded} setSidebar={setSidebarExpanded}/>
      <span className={Style.AppLayoutView}>
        <div className={sidebarExpanded ? Style.pageWithNavExpanded : Style.pageWithNavNotExpanded}>
          {props ? props.view : null}
        </div>
      </span>
    </div>
  );
}

interface Props {
  view : JSX.Element
}
