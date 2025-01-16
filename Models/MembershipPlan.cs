using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class MembershipPlan
{
    public decimal PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public decimal PDuration { get; set; }

    public decimal Price { get; set; }

    public string? Details { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
