using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class EnrollmentModel
    {
        [Key]
        public int EnrollID { get; set; }

        [Required(ErrorMessage = "Patient reference is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Program reference is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Program ID must be a positive integer.")]
        public int ProgramID { get; set; }

        [Required]
        public DateTime EnrolledAt { get; set; }

        [Required]
        public int EnrolledBy { get; set; }

        [Required(ErrorMessage = "Consent document URI is required.")]
        [Url(ErrorMessage = "Consent document must be a valid URL.")]
        public string ConsentDocumentURI { get; set; } = string.Empty;

        [Required]
        public EnrollmentStatus Status { get; set; }
    }
}