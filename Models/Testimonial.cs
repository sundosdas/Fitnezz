using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Testimonial
{
    public decimal TestimonialId { get; set; }

    public decimal UserId { get; set; }

    public string TContent { get; set; } = null!;

    public string? IsApproved { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public virtual Userr User { get; set; } = null!;
}
