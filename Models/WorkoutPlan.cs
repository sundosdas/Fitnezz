using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class WorkoutPlan
{
    public decimal WorkoutId { get; set; }

    public decimal TrainerId { get; set; }

    public decimal MemberId { get; set; }

    public string? Details { get; set; }

    public string? Goals { get; set; }

    public virtual Userr Member { get; set; } = null!;

    public virtual Userr Trainer { get; set; } = null!;
}
