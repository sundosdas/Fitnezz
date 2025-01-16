using System.ComponentModel.DataAnnotations.Schema;

namespace gym.Models
{
    public class UserForm
    {
        public decimal UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? PicPath { get; set; }

        [NotMapped]
        public virtual IFormFile? ImageFile { get; set; }

        public string? Address { get; set; }

        public decimal RoleId { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
