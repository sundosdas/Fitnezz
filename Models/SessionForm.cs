namespace gym.Models
{
    public class SessionForm
    {
        public decimal SessionId { get; set; }

        public decimal MemberId { get; set; }

        public decimal TrainerId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public string Status { get; set; } = null!;
    }
}
