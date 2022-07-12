import { AuthView } from "../Views/AuthView";

export default [
  {
    path: '/login',
    view: AuthView,
    permission: 'Public',
    title: 'Login',
  }
]
