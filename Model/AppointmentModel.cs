using System.ComponentModel.DataAnnotations;

namespace TeleCare.Model
{
    public class Appointment
    {
        [Key]
        public int AppID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int ClinicianID { get; set; }

        [Required]
        public DateTime ScheduledAt { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required]
        public string Mode { get; set; } = string.Empty;

        public string? LocationURI { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
