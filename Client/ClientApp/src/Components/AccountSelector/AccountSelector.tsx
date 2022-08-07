import { ApiReq } from "../../Utils/Hooks/ApiReq";
import React from "react";
import { Pill, PillColours, PillSizes } from "../Pill/Pill";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FaIcon, FaIconSize } from "../../Utils/fontAwesomeIcons";
import Style from './AccountSelector.module.scss'
import axios from "axios";
import { SwitchAccounts } from "../../App/AuthProvider";

interface Props extends React.HTMLAttributes<HTMLBaseElement> {
}

interface Account {
  accountId: string;
  name: string;
}


// TODO: Pick up here, on click of the pill, the account is selected. Get a new token for the account.
function DropDownItem(props: { account: Account }) {
    return (
      <div style={{marginTop: "10%"}}>
        <Pill asButton={true} onClick={async () => await SwitchAccounts(props.account.accountId)} color={PillColours.Pink} size={PillSizes.Small}>
          {props.account.name}
        </Pill>
      </div>
    )
  }

  export function AccountSelector(props: Props) {
    const [accounts, setAccounts] = React.useState<Account[]>([]);
    const [showAccountSelector, setShowAccountSelector] = React.useState(false);


    React.useEffect(() => {
      ApiReq("/api/user/getUserAccounts").then(res => {
        setAccounts(res.data);
      });
    }, []);

    function toggleHidden() {
      setShowAccountSelector(!showAccountSelector);
    }

    return (
      <div>
        {accounts.length > 1 ?
          <div>
            <Pill asButton={true} onClick={toggleHidden} size={PillSizes.Medium} color={PillColours.Green}>
              <FontAwesomeIcon icon={FaIcon.User} size={FaIconSize.SizeS} style={{paddingRight: "10px"}}/>
              Switch Accounts

            </Pill>
            <div className={Style.accountSelectContainer}
                 style={{alignContent: "center", visibility: showAccountSelector ? "visible" : "hidden"}}>
              {accounts.map(account => <DropDownItem account={account}/>)}
            </div>
          </div>
          :
          null
        }
      </div>
    );
  }
