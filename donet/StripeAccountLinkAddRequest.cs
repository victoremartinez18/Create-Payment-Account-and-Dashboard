
{
    public class StripeAccountLinkAddRequest
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string RefreshUrl { get; set; }
        [Required]
        public string ReturnUrl { get; set; }


    }
}
