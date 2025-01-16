namespace gym.Models
{
    public class abUS
    {
        public decimal AbModificationid { get; set; }
        public string Headertitle { get; set; } = null!;
        public string? Paragraph1 { get; set; }
        public string? Paragraph2 { get; set; }
        public string? Paragraph3 { get; set; }
        // IMAGE PATH
        public string? Image1 { get; set; } 
        public string? Image2 { get; set; }
        // IMAGE FILES
        public IFormFile? ImageFile1 { get; set; } 
        public IFormFile? ImageFile2 { get; set; }
    }
}
