namespace TeleCare.DTO
{
    public class AuditorVisitNoteResponseDto
    {
        public string PatientName { get; set; } = string.Empty;
        public required string Notes { get; set; }
        public required string Diagnosis { get; set; }
        public required string Orders { get; set; }
        public required string AttachmentName { get; set; }
        public required string VisitNoteStatus { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class SearchVisitNoteDto
    {
        public string? PatientName { get; set; }
        public string? VisitNoteStatus { get; set; }
        public string? SearchText { get; set; }
    }
}
