
{
    public class PaymentAccount
    {
        public int Id { get; set; }
        public LookUp Venue {  get; set; }  
        public string AccountId { get; set; }
        public LookUp PaymentType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser CreatedBy { get; set; }
        public BaseUser ModifiedBy { get; set; }
    }
}
