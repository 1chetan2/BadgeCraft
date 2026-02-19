using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BadgeCraft_Net.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Organization is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Organization Id")]
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public Organization? Organization { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("OrgUser|OrgAdmin|User",
            ErrorMessage = "Role must be OrgUser, OrgAdmin or User")]
        public string Role { get; set; } = "OrgUser";
    }
}
