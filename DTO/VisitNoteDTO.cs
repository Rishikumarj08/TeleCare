using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class VisitNoteDto
    {
        public int VisitNoteId { get; set; }

        public int PatientReferenceNumber { get; set; }

        public required string Notes { get; set; }

        public required string Diagnosis { get; set; }

        public required string Orders { get; set; }

        public required string AttachmentName { get; set; }

        public VisitNoteStatusEnum VisitNoteStatus { get; set; }
    }
}
