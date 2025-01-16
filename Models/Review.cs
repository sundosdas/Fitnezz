using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Review
{
    public decimal ReviewId { get; set; }

    public decimal SessionId { get; set; }

    public string FeedbackText { get; set; } = null!;

    public decimal? Rating { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public virtual Session Session { get; set; } = null!;
}
