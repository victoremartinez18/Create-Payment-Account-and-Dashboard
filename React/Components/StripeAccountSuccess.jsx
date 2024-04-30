import React from "react";
import { useNavigate } from "react-router-dom";

import { Container, Row, Col } from "react-bootstrap";

function StripeCreateAccountSuccess() {
  const navigate = useNavigate();

  setTimeout(() => {
    if (sessionStorage.getItem("paymentAccount")) {
      navigate(`/paymentaccount?id=${sessionStorage.getItem("paymentAccount")}`);
    }
  }, 3000);

  return (
    <React.Fragment>
      <div className="py-lg-18 py-10 bg-auto">
        <Container>
          <Row className="justify-content-center">
            <Col xl={10} lg={10} md={12}>
              <div className="py-8 py-lg-0 text-center">
                <h1 className="display-2 fw-bold mb-3 text-primary">
                  <span className="text-primary px-3 px-md-0">Completing Account Setup</span>
                </h1>

                <p className="mt-4 h2 text-dark">Thank you for your patience</p>
              </div>
            </Col>
          </Row>
        </Container>
      </div>
    </React.Fragment>
  );
}

export default StripeCreateAccountSuccess;
