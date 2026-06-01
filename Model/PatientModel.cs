using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class PatientModel
    {
        [Key]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "UserID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserID must be a positive integer")]
        public int UserID { get; set; }


        [Required(ErrorMessage = "DOB is required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "MRN is required")]
        [StringLength(20, MinimumLength = 3)]
        public required string MRN { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public required string Name { get; set; }
            [Required(ErrorMessage = "Gender is required")]
            public required string Gender { get; set; }

            public required string ContactInfoJSON { get; set; }
            public required string Address { get; set; }
            public required string PrimaryLanguage { get; set; }
            public required string EmergencyContactJSON { get; set; }
        public bool ConsentStatus { get; set; }
        public string EnrolledProgramsJSON { get; set; }
        public PatientStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}