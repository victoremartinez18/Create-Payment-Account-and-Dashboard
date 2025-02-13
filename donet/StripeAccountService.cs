
{
    public class StripeAccountService : IStripeAccountService
    {

        #region CREATE TEST ACCOUNT
        public string CreateTestAccount()
        {
            var options = new AccountCreateOptions
            {
                Type = "custom",
                Country = "US",
                // Email = "email address to identify account.",
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions
                    {
                        Requested = true,
                    },
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true
                    },
                },
            };

            var service = new AccountService();
            Account account = service.Create(options);

            return account.Id;
        }
        #endregion

        #region CREATE LINK 
        public string CreateLink(StripeAccountLinkAddRequest model)
        {
            var options = new AccountLinkCreateOptions
            {
                Account = model.Account,
                RefreshUrl = "https://localhost:3000" + $"{model.RefreshUrl}",// for development
                ReturnUrl = "https://localhost:3000" + $"{model.ReturnUrl}", // for development
                Type = "account_onboarding", // for development
            };
            var service = new AccountLinkService();
            AccountLink accountLink = service.Create(options);

            return accountLink.Url;
        }
        #endregion

        #region GET BALANCE 
        public Balance GetBalance(string accountId)
        {
            var requestOptions = new RequestOptions()
            {
                StripeAccount = accountId
            };

            var service = new BalanceService();
            Balance balanceObj = service.Get(requestOptions);

            return balanceObj;
        } 
        #endregion

    }
}
