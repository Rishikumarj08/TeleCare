using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class AlertDto
    {
        public int AlertId { get; set; }

        public int PatientReferenceNumber { get; set; }

        public string AlertType { get; set; }

        public AlertSeverityEnum AlertSeverity { get; set; }

        public string Message { get; set; }

        public AlertStatusEnum AlertStatus { get; set; }
    }
}
