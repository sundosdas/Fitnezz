using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Contactuspage
{
    public decimal CModificationid { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? Lastupdated { get; set; }

    public decimal Adminid { get; set; }

    public virtual Userr Admin { get; set; } = null!;
}
