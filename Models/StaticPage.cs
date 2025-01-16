using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class StaticPage
{
    public decimal PageId { get; set; }

    public string Title { get; set; } = null!;

    public string PageContent { get; set; } = null!;

    public DateTime? LastUpdated { get; set; }
}
