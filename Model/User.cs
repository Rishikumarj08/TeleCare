using System.ComponentModel.DataAnnotations;

namespace TeleCare.Model
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public required string Name { get; set; }

        public int RoleID { get; set; }

        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required string PasswordHash { get; set; }

        public bool MFAEnabled { get; set; }

        public string? Pin { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Role? Role { get; set; }
    }
}