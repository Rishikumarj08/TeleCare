namespace TeleCare.DTO
{
    public class AuditLogResponseDto
    {
        public required string Action { get; set; }
        public required string ResourceType { get; set; }
        public string PerformedBy { get; set; } = string.Empty;
        public string? DetailsJSON { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SearchAuditLogDto
    {
        public string? Action { get; set; }
        public string? ResourceType { get; set; }
        public string? PerformedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
