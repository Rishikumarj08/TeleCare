using System.ComponentModel.DataAnnotations;

namespace TeleCare.DTO
{
    public class VisitNoteCreateDto
    {
        [Required]
        public int AppID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int ClinicianID { get; set; }

        [Required(ErrorMessage = "Note text is required")]
        public string NoteText { get; set; }

        public string? DiagnosesJSON { get; set; }

        public string? OrdersJSON { get; set; }

        public string? AttachmentsURIJSON { get; set; }
    }

    public class VisitNoteResponseDto
    {
        public int NoteID { get; set; }

        public int AppID { get; set; }

        public int PatientID { get; set; }

        public int ClinicianID { get; set; }

        public string NoteText { get; set; }

        public string? DiagnosesJSON { get; set; }

        public string? OrdersJSON { get; set; }

        public string? AttachmentsURIJSON { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class VisitNoteQueryDto
    {
        public int? PatientID { get; set; }

        public int? ClinicianID { get; set; }

        public string? SearchText { get; set; }
    }
}