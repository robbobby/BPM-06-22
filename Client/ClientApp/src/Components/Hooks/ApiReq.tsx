import axios from "axios";
import { SetToken, GetToken, Token } from "../../App/AuthProvider";

export async function ApiReq(url: string, method: string = "get", data?: any) {
  const userToken: Token | undefined = await GetToken();

  return axios({
    url: process.env.REACT_APP_API_URL + url,
    method: method,
    data: data,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${userToken?.accessToken}`
    }
  });
}
