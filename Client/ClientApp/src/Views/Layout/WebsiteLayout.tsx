import { SideNavBar } from "../../Components/SideNavBar/SideNavBar";
import { Header } from "../../Components/Header/Header";
import { DashboardView } from "../DashboardView";
import Style from './WebsiteLayoutView.module.scss';
import { useTheme } from "../../Components/Hooks/UseTheme";
import { useState } from "react";

export function WebsiteLayout(props: Props) {
  useTheme();
  
  return (
    <div style={{height: "100%"}}>
    <Header/>
      <span className={Style.AppLayoutView}>
        <div>
          {props ? props.view : null}
        </div>
      </span>
    </div>
  );
}

interface Props {
  view : JSX.Element
}
