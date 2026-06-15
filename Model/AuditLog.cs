using System.ComponentModel.DataAnnotations;

namespace TeleCare.Model
{
    public class AuditLog
    {
        [Key]
        public int AuditID { get; set; }
        public int UserID { get; set; }
        public required string Action { get; set; }
        public required string ResourceType { get; set; }
        public int? ResourceID { get; set; }
        public string? DetailsJSON { get; set; }
        public DateTime Timestamp { get; set; }

        public User? PerformedBy { get; set; }
    }
}
