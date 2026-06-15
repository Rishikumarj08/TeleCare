namespace TeleCare.DTO
{
    public class AlertCreateDto
    {
        public int PatientID { get; set; }

        public int RuleID { get; set; }

        public DateTime TriggeredAt { get; set; }

        public string Severity { get; set; } = string.Empty;

        public int AssignedToFK { get; set; }

        public DateTime? AcknowledgedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }
        
        public string Status { get; set; } = string.Empty;
    }

    public class AlertResponseDto
    {
        public int AlertID { get; set; }

        public int PatientID { get; set; }

        public int RuleID { get; set; }

        public DateTime TriggeredAt { get; set; }

        public string Severity { get; set; } = string.Empty;

        public int AssignedToFK { get; set; }

        public DateTime? AcknowledgedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }
        
        public string Status { get; set; } = string.Empty;
    }
}