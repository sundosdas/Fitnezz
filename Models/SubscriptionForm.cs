using System.ComponentModel.DataAnnotations.Schema;

namespace gym.Models
{
    public class SubscriptionForm
    {
        public decimal SubscriptionId { get; set; }
        public decimal UserId { get; set; }
        public decimal PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
