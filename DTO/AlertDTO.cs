using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class AlertDto
    {
        public int AlertId { get; set; }

        public int PatientReferenceNumber { get; set; }

        public required string AlertType { get; set; }

        public AlertSeverityEnum AlertSeverity { get; set; }

        public required string Message { get; set; }

        public AlertStatusEnum AlertStatus { get; set; }
    }
}
