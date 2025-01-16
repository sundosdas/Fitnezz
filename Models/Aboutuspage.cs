using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace gym.Models;

public partial class Aboutuspage
{
    public decimal AbModificationid { get; set; }

    public string Headertitle { get; set; } = null!;

    public string? Paragraph1 { get; set; }

    public string? Paragraph2 { get; set; }

    public string? Paragraph3 { get; set; }

    public string? Image1 { get; set; }

    [NotMapped]
    public virtual IFormFile? ImageFile1 { get; set; }

    public string? Image2 { get; set; }

    [NotMapped]
    public virtual IFormFile? ImageFile2 { get; set; }
    public DateTime? Lastupdated { get; set; }

    public decimal Adminid { get; set; }

    public virtual Userr Admin { get; set; } = null!;
}
