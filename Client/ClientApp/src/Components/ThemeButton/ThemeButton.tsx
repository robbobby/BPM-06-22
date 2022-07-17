import useDarkMode from "use-dark-mode";
import { useTheme } from "../Hooks/UseTheme";
import './ThemeButton.scss'
import { DarkModeSwitch } from "react-toggle-dark-mode";

export function ThemeButton() {
  const darkMode = useDarkMode(true);
  const theme = useTheme();

  return (
    <div style={{padding: "10px"}}>
      <DarkModeSwitch onChange={() => {}}
                      checked={true}
                      size={50}
                      sunColor={"orange"}
                      moonColor={"blue"}/>
    </div>
  );
}
