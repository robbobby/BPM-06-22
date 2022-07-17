import React from 'react';
import { createContext, useState } from 'react';
import { Navigate } from "react-router-dom";
import RolePermissions from "./RolePermissions";
import jwtDecode from "jwt-decode";
import { AppLayoutView } from "../Views/Layout/AppLayoutView";

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

export function LogOut() {
    localStorage.removeItem('user');
}

export function SetToken(token: TokenResponse) {
    const jwtValues = jwtDecode<TokenValues>(token.accessToken);

    const user: UserLocalStorage = {
        accessToken: token.accessToken,
        refreshToken: token.refreshToken,
        accessTokenExpiration: jwtValues.exp,
        role: jwtValues.role
    }
    localStorage.setItem("user", JSON.stringify(user));
}

export function PrivateRoute(props: PrivateRouteProps) {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const layout = props.layout || "App";
    if(RolePermissions.permissions[user.role][props.permission]) {
        switch (layout) { 
            case 'App':
                return <AppLayoutView view={props.view}/>;
            default:
                throw new Error(`Unknown layout: ${layout}`);
        }
    } else {
        return <Navigate to="/login"/>;
    }
}

interface PrivateRouteProps {
    layout: string;
    view: JSX.Element
    permission: string
}

export interface TokenResponse {
    accessToken: string;
    refreshToken: string;
}

interface TokenValues {
    role: string,
    exp: number
}

interface UserLocalStorage {
    accessToken?: string;
    refreshToken?: string;
    accessTokenExpiration?: number;
    role: string;
}