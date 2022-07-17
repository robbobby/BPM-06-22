import { SideNavBar } from "../../Components/SideNavBar/SideNavBar";
import { Header } from "../../Components/Header/Header";
import { DashboardView } from "../DashboardView";
import Style from './AppLayoutView.module.scss';

export function AppLayoutView(props: any) {
  return (
    <div style={{height: "100%"}}>
    <Header/>
      <span className={Style.AppLayoutView}>
        <SideNavBar/>
        <div>
          <DashboardView/>
        </div>
      </span>
    </div>
  );
}