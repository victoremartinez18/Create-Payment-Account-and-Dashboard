using Sabio.Models.Requests;
using Stripe;

namespace Sabio.Services.Interfaces
{
    public interface IStripeAccountService
    {
        string CreateTestAccount();
        public string CreateLink(StripeAccountLinkAddRequest model);
        Balance GetBalance(string accountId);
    }
}