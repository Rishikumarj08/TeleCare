using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class Alert
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient reference number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient reference number must be greater than zero.")]
        public int PatientReferenceNumber { get; set; }

        [Required(ErrorMessage = "Alert type is required.")]
        public required string AlertType { get; set; }

        [Required(ErrorMessage = "Alert severity is required.")]
        public AlertSeverityEnum AlertSeverity { get; set; }

        [Required(ErrorMessage = "Alert message is required.")]
        public required string Message { get; set; }

        [Required(ErrorMessage = "Alert status is required.")]
        public AlertStatusEnum AlertStatus { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}