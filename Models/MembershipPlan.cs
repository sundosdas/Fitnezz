using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gym.Models;

public partial class MembershipPlan
{
    public decimal PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public decimal PDuration { get; set; }

    public decimal Price { get; set; }

    public string? Details { get; set; }

    public int? WorkoutsNum { get; set; } = 1;
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
