import React from 'react';
import './App.scss';
import AuthProvider, { PrivateRoute } from './App/AuthProvider';
import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { Header } from "./Components/Header/Header";

const routeConfigs = [
  ...require('./Routes/AccountRoutes').default,
  ...require('./Routes/AuthRoutes').default,
  ...require('./Routes/Routes').default
];

function PageNotFound() {
  // get user from local storage
  const user = JSON.parse(localStorage.getItem('user')!);
  if (user) 
    return <Navigate to="/dashboard"/>;
  
  return <Navigate to={'/login'}/>;
}

function App() {
  // switch between login and signup view
  const [loginView, setLoginView] = React.useState(true);

  let routes = [];

  function getRoutes() {
    routes = routeConfigs.map(routeConfig => {
      return (
        <Route key={routeConfig.path} path={routeConfig.path} element={
          routeConfig.permission !== 'Public' ?
            <PrivateRoute 
              layout={routeConfig.layout} 
              view={routeConfig.view()} 
              permission={routeConfig.permission}
            /> 
            : routeConfig.view()}
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
            <Route path='*' element={<PageNotFound/>}/>
            {getRoutes()}
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </div>
  );
}

export default App;
