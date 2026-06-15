using System.ComponentModel.DataAnnotations;

namespace TeleCare.Model
{
    public class VisitNote
    {
        [Key]
        public int NoteID { get; set; }

        [Required]
        public int AppID { get; set; }

        [Required]
        public int PatientID { get; set; }

        [Required]
        public int ClinicianID { get; set; }

        [Required]
        public string NoteText { get; set; }

        public string? DiagnosesJSON { get; set; }

        public string? OrdersJSON { get; set; }

        public string? AttachmentsURIJSON { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}