import React, { useState, useEffect } from "react";
import { toast } from "react-toastify";

import stripeAccountService from "services/stripeAccountService";
import paymentAccountService from "services/paymentAccountService";


const StripeAccountCreateBtn = () => {
  const [linkData, setLinkData] = useState({
    account: "",
    type: "account_onboarding",
    refreshUrl: "/paymentaccount",
    returnUrl: "/stripeaccountsuccess",
  });

  const [accountData, setAccountData] = useState({
    venueId: 2, //Hardcoded at the moment, but venue Id will be set dinamically in the future.
    accountId: "",
    paymentTypeId: 1,
  });

  useEffect(() => {
    if (linkData.account !== "") {
      getNewLink(linkData);

      paymentAccountService
        .addAccount(accountData)
        .then(onAddAccountSuccess)
        .catch(onAddAccountError);
    }
  }, [linkData.account]);

  function onAddAccountSuccess(response) {
    _logger("Add Payment account success", response);
    let id = response.item;
    sessionStorage.setItem("paymentAccount", id);
  }

  function onAddAccountError(error) {
    _logger("Create account error", error);

    toast.error("Error creating account.", {
      position: "top-right",
    });
  }

  function onCreateBtnClicked() {
    _logger("CreateBtn Clicked!!!");

    stripeAccountService
      .createTestAccount()
      .then(onCreateAccountSuccess)
      .catch(onCreateAccountError);
  }

  const getNewLink = (payload) => {
    _logger("Payload", payload);
    stripeAccountService
      .createAccountLink(payload)
      .then(onCreateLinkSuccess)
      .catch(onCreateLinkError);
  };

  function onCreateLinkSuccess(response) {
    _logger("Creating a link has succeded", response);
    let url = response.item;
    window.location.href = url;
  }

  function onCreateLinkError(error) {
    _logger("Creating a link has failes", error);
  }

  function onCreateAccountSuccess(response) {
    _logger(response.item);
    let id = response.item;

    setAccountData((prevState) => {
      let newState = { ...prevState };
      newState.accountId = id;
      return newState;
    });

    setLinkData((prevState) => {
      let newState = { ...prevState };
      newState.account = id;
      return newState;
    });
  }

  function onCreateAccountError(error) {
    _logger("Create account failed", error);
  }

  return (
    <React.Fragment>
      <form method="POST" className={`align-center card p-3 w-25 m-5 mx-5`}>
        <button
          type="button"
          id="stripe-button"
          className={`btn btn-primary`}
          onClick={onCreateBtnClicked}
        >
          Create Account
        </button>
      </form>
    </React.Fragment>
  );
};

export default StripeAccountCreateBtn;
