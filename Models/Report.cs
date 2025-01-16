using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Report
{
    public decimal ReportId { get; set; }

    public string? ReportTitle { get; set; }

    public string? ReportType { get; set; }

    public string? PdfPath { get; set; }
}
