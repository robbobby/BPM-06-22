import React from 'react';
import { createContext, useState } from 'react';
import { Navigate } from "react-router-dom";
import RolePermissions from "./RolePermissions";
import jwtDecode from "jwt-decode";
import { AppLayoutView } from "../Views/Layout/AppLayoutView";
import axios, { AxiosResponse } from "axios";
import { ApiReq } from "../Components/Hooks/ApiReq";

export async function GetToken(): Promise<Token | undefined> {
    const user = localStorage.getItem('user');

    if (user) {
        const userToken: Token = JSON.parse(user);
        if (jwtDecode<ParsedToken>(userToken.accessToken).exp < Date.now() / 1000) {
            return RefreshToken(userToken);
        } else {
            return userToken;
        }
    }
    return undefined;
}

export async function SwitchAccounts(accountId: string) {
    const token = await GetToken();
    if (token) {
        
        ApiReq("/api/auth/switchAccount", "post", accountId).then(res => {
            SetLocalStorageUserFromToken(res.data);
            window.location.reload();
        });
    }
}

async function RefreshToken(token: Token): Promise<Token> {
    const res: AxiosResponse<Token> = await axios({
        url: process.env.REACT_APP_API_URL + "/api/auth/refreshToken",
        method: "post",
        data: {
            accessToken: token.accessToken,
            refreshToken: token.refreshToken
        },
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token.accessToken}`
        }
    });

    SetLocalStorageUserFromToken(res.data);
    return res.data;
}

export const AuthContext = React.createContext({});

export default function AuthProvider(props: any) {
    const cache = JSON.parse(localStorage.getItem('user') || '{}');
    const [user, setUser] = useState(cache);

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
    return <Navigate to="/login"/>;
}

export function SetLocalStorageUserFromToken(token: Token) {
    console.log(token.accessToken);
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
    const user: any = JSON.parse(localStorage.getItem('user')!);
    const layout = props.layout || "App";
    if (!user) {
        return <Navigate to="/login"/>;
    }
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

export interface Token {
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

export type ParsedToken = {
    role: string;
    UserId: string
    AccountId: string
    nbf: number;
    exp: number
    iat: number;
    iss: string;
    aud: string;
}
