using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class VisitNote
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient reference number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient reference number must be greater than zero.")]
        public int PatientReferenceNumber { get; set; }

        [Required(ErrorMessage = "Notes are required.")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Orders are required.")]
        public string Orders { get; set; }

        public string AttachmentName { get; set; }

        [Required(ErrorMessage = "Visit note status is required.")]
        public VisitNoteStatusEnum VisitNoteStatus { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
