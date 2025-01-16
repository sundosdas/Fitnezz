using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Schedule
{
    public decimal ScheduleId { get; set; }

    public decimal TrainerId { get; set; }

    public string AvailableFrom { get; set; } = null!;

    public string AvailableTo { get; set; } = null!;

    public decimal SessionId { get; set; }

    public string DayOfWeek { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;

    public virtual Userr Trainer { get; set; } = null!;
}
