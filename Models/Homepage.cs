using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace gym.Models;

public partial class Homepage
{
    public decimal HModificationid { get; set; }

    public string? Slidertitle1 { get; set; } = null!;

    public string? Slidersubtitle1 { get; set; }

    public string? Sliderimage1 { get; set; }

    [NotMapped]
    public virtual IFormFile? ImageFileS1 { get; set; }

    public string? Slidertitle2 { get; set; } = null!;

    public string? Slidersubtitle2 { get; set; }

    public string? Sliderimage2 { get; set; }
    
    [NotMapped]
    public virtual IFormFile? ImageFileS2 { get; set; }

    public DateTime? Lastupdated { get; set; }

    public decimal Adminid { get; set; }

    public virtual Userr Admin { get; set; } = null!;
}
