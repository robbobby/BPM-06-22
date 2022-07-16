import React from "react";
import useDarkMode from "use-dark-mode";

export const useTheme = () => {
  const lightTheme = "light-mode";
  const darkTheme = "dark-mode";

  const darkMode = useDarkMode();

  React.useEffect(() => {
    setTheme(darkMode.value ? darkTheme : lightTheme);
  }, [darkMode.value]);

  const [theme, setTheme] = React.useState(darkTheme);
  return theme;
}