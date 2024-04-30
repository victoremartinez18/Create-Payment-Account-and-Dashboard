using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Stripe;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/stripeaccount")]
    [ApiController]
    public class StripeAccountApiController : BaseApiController
    {
        private AppKeys _appKeys = null;
        private IStripeAccountService _service = null;

        public StripeAccountApiController(IOptions<AppKeys> appKeys,
            IStripeAccountService service,
            ILogger<StripeAccountApiController> logger) : base(logger)
        {
            _service = service;
            _appKeys = appKeys.Value;

            StripeConfiguration.ApiKey = _appKeys.StripeSecretApiKey;
        }

        [HttpPost("createaccount")]
        public ActionResult<ItemResponse<string>> CreateTestAccount()
        {

            ObjectResult result = null;
            try
            {
                string id = _service.CreateTestAccount();

                ItemResponse<string> response = new ItemResponse<string>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPost("createlink")]
        public ActionResult<ItemResponse<string>> CreateLink(StripeAccountLinkAddRequest model)
        {

            ObjectResult result = null;
            try
            {
                string link = _service.CreateLink(model);

                ItemResponse<string> response = new ItemResponse<string>() { Item = link };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("balance")]
        public ActionResult<ItemResponse<Balance>> Get(string id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Balance balance = _service.GetBalance(id);

                if (balance == null)
                {
                    code = 400;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Balance> { Item = balance };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

    }
}
