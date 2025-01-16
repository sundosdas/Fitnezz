using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Contactusform
{
    public decimal Formid { get; set; }

    public string Username { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? Submittedat { get; set; }

    public string? Isreviewed { get; set; }
}
