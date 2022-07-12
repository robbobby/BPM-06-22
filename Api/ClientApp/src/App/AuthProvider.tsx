import React from 'react';
import { createContext, useState } from 'react';
import { Navigate } from "react-router-dom";
import RolePermissions from "./RolePermissions";

export const AuthContext = React.createContext({});
export default function AuthProvider(props: any) {
    const cache = JSON.parse(localStorage.getItem('user') || '{}');
    const [user, setUser] = useState(cache);

    console.log(props);
    const login = (user: any) => {
        setUser(user);
        localStorage.setItem('user', JSON.stringify(user));
    };

    const logout = (res: any) => {
        setUser(null);
        localStorage.removeItem('user');
    };

    return (
        <AuthContext.Provider
            value={{ user, login, logout }}
            {...props}/>
    );
}

export function PrivateRoute(props: PrivateRouteProps) {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    if(RolePermissions.permissions[user.role][props.permission]) {
        return props.view;
    } else {
        return <Navigate to="/login"/>;
    }
}

interface PrivateRouteProps {
    view: JSX.Element
    permission: string
}
