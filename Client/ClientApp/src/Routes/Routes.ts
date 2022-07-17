import { DashboardView } from "../Views/DashboardView";
import { AppLayoutView } from "../Views/Layout/AppLayoutView";

export default [
  {
    path: '/dashboard',
    view: DashboardView,
    permission: 'User',
    title: 'Dashboard',
  },
]
