import axios from "axios";

export function ApiReq(url: string, method: string = "get", data?: any) {
  return axios({
    url: process.env.REACT_APP_API_URL + url,
    method: method,
    data: data,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    }
  });
}