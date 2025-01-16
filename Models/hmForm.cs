using System.ComponentModel.DataAnnotations;

namespace gym.Models
{
    public class hmForm
    {
        public decimal? HModificationid { get; set; }
        [Required]
        [Display(Name = "Slider Title 1")]
        public string Slidertitle1 { get; set; } = null!;

        [Display(Name = "Slider Subtitle 1")]
        public string? Slidersubtitle1 { get; set; }

        [Display(Name = "Slider Image 1")]
        public IFormFile? ImageFileS1 { get; set; } 

        public string? ExistingImage1 { get; set; } 

        [Required]
        [Display(Name = "Slider Title 2")]
        public string Slidertitle2 { get; set; } = null!;

        [Display(Name = "Slider Subtitle 2")]
        public string? Slidersubtitle2 { get; set; }

        [Display(Name = "Slider Image 2")]
        public IFormFile? ImageFileS2 { get; set; }

        public string? ExistingImage2 { get; set; } 
        public DateTime? Lastupdated { get; set; }

        [Display(Name = "Admin ID")]
        public decimal Adminid { get; set; }
    }
}
