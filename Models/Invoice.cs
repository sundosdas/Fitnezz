namespace gym.Models
{
    public class Invoice
    {
        public string PaymentReference { get; set; } 
        public decimal PaymentAmount { get; set; } 
        public DateTime PaymentDate { get; set; } 
        public string InvoicePath { get; set; }    
        public Subscription Subscription { get; set; }  
        public decimal SubscriptionId { get; set; }  
    }

}
