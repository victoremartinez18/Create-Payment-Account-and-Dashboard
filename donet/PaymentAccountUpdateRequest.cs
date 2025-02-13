
{
    public class PaymentAccountUpdateRequest : PaymentAccountAddRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
    }
}
