using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class VisitNoteQueryDto
    {
        public string? SearchText { get; set; }

        public int? PatientReferenceNumber { get; set; }

        public VisitNoteStatusEnum? VisitNoteStatus { get; set; }
    }
}