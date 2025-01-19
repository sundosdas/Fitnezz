namespace gym.Models
{
    public class ScheduleForm
    {
        public decimal ScheduleId { get; set; }

        public decimal TrainerId { get; set; }

        public string AvailableFrom { get; set; } = null!;

        public string AvailableTo { get; set; } = null!;

        public decimal SessionId { get; set; }

        public string DayOfWeek { get; set; } = null!;
    }
}
