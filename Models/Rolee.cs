using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class Rolee
{
    public decimal RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Userr> Userrs { get; set; } = new List<Userr>();
}
