import {useTheme} from "../Components/Hooks/UseTheme";
import { SideNavBar } from "../Components/SideNavBar/SideNavBar";

export function DashboardView(props: any) {
  const navLinks = [
    {label: "Hello",
      link: "/hello",
      icon: "home"},
    {label: "Hello",
      link: "/hello",
      icon: "home"},
  ]
    return (
        <div>
            <h1>Dashboard</h1>
        </div>
    );
}
