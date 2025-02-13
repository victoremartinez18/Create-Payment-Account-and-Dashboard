
{
    [Route("api/paymentaccount")]
    [ApiController]
    public class PaymenAccountApiController : BaseApiController
    {
        private IPaymentAccountService _service = null;
        private IAuthenticationService<int> _authService = null;
        public PaymenAccountApiController(IPaymentAccountService service,
            IAuthenticationService<int> authService,
            ILogger<PaymenAccountApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(PaymentAccountAddRequest model)
        {
            ObjectResult result = null;
            int id = 0;

            try
            {
                int createdBy = _authService.GetCurrentUserId();
                id = _service.Add(model, createdBy);

                if (id == 0)
                {
                    ErrorResponse response = new ErrorResponse("Item not created.");
                    result = StatusCode(500, response);
                }
                else
                {
                    ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                    result = Created201(response);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
                result = StatusCode(500, response);

            }

            return result;

        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<PaymentAccount>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                PaymentAccount account = _service.GetById(id);

                if (account == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not Found");
                }
                else
                {
                    response = new ItemResponse<PaymentAccount>() { Item = account };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<PaymentAccount>>> GetPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            if (pageIndex >= 0 && pageSize > 0)
            {

                try
                {
                    Paged<PaymentAccount> paged = _service.Get(pageIndex, pageSize);

                    if (paged == null || paged?.TotalCount == 0)
                    {
                        code = 404;
                        response = new ErrorResponse("Resource not found.");
                    }
                    else
                    {
                        response = new ItemResponse<Paged<PaymentAccount>> { Item = paged };
                    }
                }
                catch (Exception ex)
                {
                    code = 500;
                    response = new ErrorResponse(ex.Message);
                    base.Logger.LogError(ex.ToString());
                }
            }

            return StatusCode(code, response);
        }

        [HttpGet("createdby")]
        public ActionResult<ItemResponse<List<PaymentAccount>>> GetByCreatedBy(int user)
        {
            int code = 200;
            BaseResponse response = null;

            if (user > 0 && user <= int.MaxValue)
            {
                try
                {
                    List<PaymentAccount> paymentAccounts = _service.GetByCreatedBy(user);

                    if (paymentAccounts == null || paymentAccounts.Count == 0)
                    {
                        code = 404;
                        response = new ErrorResponse("Resource not found.");
                    }
                    else
                    {
                        response = new ItemResponse<List<PaymentAccount>> { Item = paymentAccounts };

                    }
                }
                catch (Exception ex)
                {
                    code = 500;
                    response = new ErrorResponse(ex.Message);
                    base.Logger.LogError(ex.ToString());
                }
            }
            else
            {
                code = 400;
                response = new ErrorResponse("Bad Request");
            }

            return StatusCode(code, response);
        }


        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(PaymentAccountUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int modifiedBy = _authService.GetCurrentUserId();
                _service.Update(model, modifiedBy);

                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }


            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            if (id > 0)
            {
                try
                {
                    _service.DeleteById(id);

                    response = new SuccessResponse();
                }
                catch (Exception ex)
                {
                    code = 500;
                    response = new ErrorResponse(ex.Message);
                    base.Logger.LogError(ex.ToString());
                }
            }
            else
            {
                code = 400;
                response = new ErrorResponse("Bad Request");
            }

            return StatusCode(code, response);
        }


    }
}
