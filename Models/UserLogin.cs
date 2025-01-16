using System;
using System.Collections.Generic;

namespace gym.Models;

public partial class UserLogin
{
    public decimal LoginId { get; set; }

    public decimal? UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public virtual Userr? User { get; set; }
}
