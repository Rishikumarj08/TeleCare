using System.ComponentModel.DataAnnotations;

namespace TeleCare.Model
{
    public class Alert
    {
        [Key]
        public int AlertID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int RuleID { get; set; }

        [Required]
        public DateTime TriggeredAt { get; set; }

        [Required]
        public string Severity { get; set; } = string.Empty;

        [Required]
        public int AssignedToFK { get; set; }

        public DateTime? AcknowledgedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
