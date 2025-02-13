import React, { useState, useEffect } from "react";
import { Row, Col, Button } from "react-bootstrap";

import "../PaymentAccount/paymentaccount.css";
import { useSearchParams } from "react-router-dom";

import paymentAccountService from "services/paymentAccountService";
import stripeAccountService from "services/stripeAccountService";
import StripeAccountCreateBtn from "./StripeAccountCreateBtn";

function PaymentAccount() {

  const [accountData, setAccountData] = useState({
    venueId: 0,
    venueName: "",
    accountId: 0,
    stripeAccountId: "",
    dateCreated: "",
    balance: null,
    balanceCurrency: "",
    available: null,
    availableCurrency: "",
    pending: null,
    pendingCurrency: "",
  });

  const [paymentAccount] = useSearchParams();

  useEffect(() => {
    if (paymentAccount.get("id") === sessionStorage.getItem("paymentAccount")) {
      paymentAccountService
        .getAccountById(paymentAccount.get("id"))
        .then(onGetByIdSuccess)
        .catch(onGetByIdError);
    } else {
      ////////// This is a Place holder. Need to determine what to do if there is no PaymentAccount passed.
      setAccountData((prevState) => {
        let newState = { ...prevState };
        newState.accountId = 0;
        newState.stripeAccountId = "No Account passed";
        newState.dateCreated = "No Account passed";

        return newState;
      });
    }

    _logger("useEffect triggered");
  }, []);

  function onGetByIdSuccess(response) {
    _logger("onGetByIdSuccess:", response.item);
    let data = response.item;

    setAccountData((prevState) => {
      let newState = { ...prevState };
      newState.accountId = data.id;
      newState.stripeAccountId = data.accountId;
      newState.dateCreated = data.dateCreated;
      newState.venueId = data.venue.id;
      newState.venueName = data.venue.name;

      return newState;
    });
  }

  function onGetByIdError(error) {
    _logger(error);
  }

  useEffect(() => {
    if (accountData.accountId !== 0) {
      stripeAccountService
        .getBalance(accountData.stripeAccountId)
        .then(onGetBalanceSuccess)
        .catch(onGetBalanceError);
    }

    _logger("useEffect 2 triggered");
  }, [accountData.accountId]);

  function onGetBalanceSuccess(response) {
    _logger("Successfully obtained account balance");
    _logger(response.item);

    let balanceObj = response.item;

    setAccountData((prevState) => {
      let newState = { ...prevState };
      newState.balance = balanceObj.available[0].amount;
      newState.balanceCurrency = balanceObj.available[0].currency;
      newState.available = balanceObj.instantAvailable[0].amount;
      newState.availableCurrency = balanceObj.instantAvailable[0].currency;
      newState.pending = balanceObj.pending[0].amount;
      newState.pendingCurrency = balanceObj.pending[0].currency;

      return newState;
    });
  }

  function onGetBalanceError(error) {
    _logger(error);
  }

  return (
    <React.Fragment>
      <Row id="payment" className="m-5 mx-auto col-9">
        <Col>
          <h3 className="mb-2 mt-4">{accountData.venueName}</h3>
          <h4>Account Id: {accountData.accountId}</h4>
        </Col>
        <Col sm={7}>
          <Row className="p-3 justify-content-center">
            <Col md="auto" className="text-end pt-2">
              <h5>Your Balance </h5>
              <h4>{accountData.balance / 100}</h4>
              <p>Currency: ({accountData.balanceCurrency})</p>
            </Col>
            <Col md="auto" id="payment-balance" className="text-end pt-2">
              <h5>Pending Balance </h5>
              <h4>{accountData.pending / 100}</h4>
              <p>Currency: ({accountData.pendingCurrency})</p>
            </Col>
            <Col md="auto" className="text-end pt-2">
              <h5>Available Funds</h5>
              <h4>{accountData.available / 100}</h4>
              <p>Currency: ({accountData.balanceCurrency})</p>
            </Col>
          </Row>
        </Col>
        <Col auto className="payoutbtn pt-2">
          <Button variant="primary" className="mt-4">
            Pay Out Now
          </Button>
        </Col>
      </Row>
      <Row className="m-5 mx-auto col-8 justify-content-center">
        <StripeAccountCreateBtn />
      </Row>
    </React.Fragment>
  );
}

export default PaymentAccount;
