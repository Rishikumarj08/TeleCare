using System.ComponentModel.DataAnnotations;

namespace TeleCare.DTO
{
    public class AppointmentCreateDto
    {
        [Required]
        public int PatientID { get; set; }

        [Required]
        public int ClinicianID { get; set; }

        [Required(ErrorMessage = "Scheduled time is required")]
        public DateTime ScheduledAt { get; set; }

        [Required]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Mode is required (Video/Phone/InPerson)")]
        public string Mode { get; set; } = string.Empty;

        public string? LocationURI { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }

    public class AppointmentResponseDto
    {
        public int AppID { get; set; }

        public int PatientID { get; set; }

        public int ClinicianID { get; set; }

        public DateTime ScheduledAt { get; set; }

        public int DurationMinutes { get; set; }

        public string Mode { get; set; } = string.Empty;

        public string? LocationURI { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }

    public class AppointmentQueryDto
    {
        public string? SearchText { get; set; }

        public string? Status { get; set; }

        public DateTime? ScheduledAt { get; set; }
    }
}