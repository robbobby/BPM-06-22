import useDarkMode from "use-dark-mode";
import { useTheme } from "../Hooks/UseTheme";
import './ThemeButton.scss'
import { DarkModeSwitch } from "react-toggle-dark-mode";

export function ThemeButton() {
  const darkMode = useDarkMode(true);
  const theme = useTheme();

  return (
      <DarkModeSwitch onChange={() => {}}
                      checked={darkMode.value}
                      size={25}
                      sunColor={"orange"}
                      moonColor={"blue"}/>
  );
  
}
