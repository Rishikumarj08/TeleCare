using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class VisitNoteDto
    {
        public int VisitNoteId { get; set; }

        public int PatientReferenceNumber { get; set; }

        public string Notes { get; set; }

        public string Diagnosis { get; set; }

        public string Orders { get; set; }

        public string AttachmentName { get; set; }

        public VisitNoteStatusEnum VisitNoteStatus { get; set; }
    }
}
