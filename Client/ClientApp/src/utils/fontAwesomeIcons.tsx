import ReactDOM from 'react-dom'
import { library } from '@fortawesome/fontawesome-svg-core'
import { fab } from '@fortawesome/free-brands-svg-icons'
import { faClipboardList, faHouse, faListCheck, faTableList, faGear, faUser, faAngleUp, faAngleDown, faAngleLeft, faAngleRight, faBlog } from '@fortawesome/free-solid-svg-icons'

library.add( faHouse, faClipboardList, faListCheck, faTableList, faGear, faUser, faAngleUp, faAngleDown, faAngleLeft, faAngleRight, faBlog )

export enum FaIcon {
  House = "house",
  ClipboardList = "clipboard-list",
  ListCheck = "list-check",
  TableList = "table-list",
  Gear = "gear",
  User = "user",
  AngleDown = "angle-down",
  AngleUp = "angle-up",
  AngleLeft = "angle-left",
  AngleRight = "angle-right",
  Blog = "blog"
}

export enum FaIconSize {
  SizeXs = "xs",
  SizeL  = "lg",
  SizeS  = "sm",
  S1  = "1x",
  S2  = "2x",
  S3  = "3x",
  S4  = "4x",
  S5  = "5x",
  S6  = "6x",
  S7  = "7x",
  S8  = "8x",
  S9  = "9x",
  S10  = "10x"
}