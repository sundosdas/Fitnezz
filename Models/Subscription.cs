using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Subscription
{
    public decimal SubscriptionId { get; set; }

    public decimal UserId { get; set; }

    public decimal PlanId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? InvoicePath { get; set; } = null!;

    public virtual MembershipPlan Plan { get; set; } = null!;

    public virtual Userr User { get; set; } = null!;

    public virtual ICollection<SubscriptionWorkout> SubscriptionWorkouts { get; set; }

}
