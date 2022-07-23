import { DashboardView } from "../Views/DashboardView";
import { AppLayoutView } from "../Views/Layout/AppLayoutView";
import { TodoView } from "../Views/TodoView";
import AccountView from "../Views/AccountView";
import { BacklogView } from "../Views/BacklogView";
import { SettingsView } from "../Views/SettingsView";

export default [
  {
    path: '/dashboard',
    view: DashboardView,
    permission: 'User',
    title: 'Dashboard',
  },
  {
    path: '/todo',
    view: TodoView,
    permission: 'User',
    title: 'Todo',
  },
  {
    path: '/backlog',
    view: BacklogView,
    permission: 'User',
    title: 'Backlog',
  },
  {
    path: '/account',
    view: AccountView,
    permission: 'User',
    title: 'Account',
  },
  {
    path: '/settings',
    view: SettingsView,
    permission: 'User',
    title: 'Settings',
  }
]
