import axios from "axios";
import { SetLocalStorageUserFromToken, GetToken, Token } from "../../App/AuthProvider";

export async function ApiReq(url: string, method: string = "get", data?: any) {
  const userToken: Token | undefined = await GetToken();

  console.log(data);
  const request =  axios({
    url: process.env.REACT_APP_API_URL + url,
    method: method,
    data: data,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${userToken?.accessToken}`
    }
  });

  console.log(request);
  
  return request;
}
