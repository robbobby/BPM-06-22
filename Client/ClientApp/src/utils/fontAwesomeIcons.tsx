import ReactDOM from 'react-dom'
import { library } from '@fortawesome/fontawesome-svg-core'
import { fab } from '@fortawesome/free-brands-svg-icons'
import { faClipboardList, faHouse, faListCheck, faTableList } from '@fortawesome/free-solid-svg-icons'

library.add( faHouse, faClipboardList, faListCheck, faTableList)

export enum FaIcon {
  House = "house",
  ClipboardList = "clipboard-list",
  ListCheck = "list-check",
  TableList = "table-list"
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