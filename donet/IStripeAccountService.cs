
{
    public interface IStripeAccountService
    {
        string CreateTestAccount();
        public string CreateLink(StripeAccountLinkAddRequest model);
        Balance GetBalance(string accountId);
    }
}
