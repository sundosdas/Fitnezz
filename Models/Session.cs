using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Session
{
    public decimal SessionId { get; set; }

    public decimal MemberId { get; set; }

    public decimal TrainerId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public string Status { get; set; } = null!;

    public virtual Userr Member { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual Userr Trainer { get; set; } = null!;
}
