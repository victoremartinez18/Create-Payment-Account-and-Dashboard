
{
    public class PaymentAccountAddRequest
    {
        [Required]
        public int VenueId { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public int PaymentTypeId { get; set; }
    }
}
