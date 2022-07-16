import { DashboardView } from "../Views/DashboardView";

export default [
  {
    path: '/dashboard',
    view: DashboardView,
    permission: 'User',
    title: 'Dashboard',
  }
]
