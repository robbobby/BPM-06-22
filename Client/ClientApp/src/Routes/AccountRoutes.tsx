import AccountView from "../Views/AccountView"

export default [
  {
    path: '/account',
    view: AccountView,
    permission: 'Admin',
    title: 'Account',
    layout: 'App'
  }
]
