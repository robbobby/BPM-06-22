import React from 'react';
import './App.scss';
import AuthProvider, { PrivateRoute } from './App/AuthProvider';
import { BrowserRouter, Route, Routes } from "react-router-dom";
import { Header } from "./Components/Header/Header";

const routeConfigs = [
  ...require('./Routes/AccountRoutes').default,
  ...require('./Routes/AuthRoutes').default,
  ...require('./Routes/Routes').default
];

function App() {
  // switch between login and signup view
  const [loginView, setLoginView] = React.useState(true);

  let routes = [];
  console.log(routeConfigs);

  function getRoutes() {
    routes = routeConfigs.map(routeConfig => {
      return (
        <Route key={routeConfig.path} path={routeConfig.path} element={
          routeConfig.permission !== 'Public' ? <PrivateRoute view={routeConfig.view()} permission={routeConfig.permission}/> : routeConfig.view()}
        />
      )
    })
    return routes;
  }

  return (
    <div>
      <AuthProvider>
        <BrowserRouter>
          {/*<Header/>*/}
          <Routes>
            {getRoutes()}
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </div>
  );
}

export default App;
