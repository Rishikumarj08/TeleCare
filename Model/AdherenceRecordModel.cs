using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class AdherenceRecordModel
    {
        [Key]
        public int AdhID { get; set; }

        [Required(ErrorMessage = "Medication ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Medication ID must be greater than zero.")]
        public int MedID { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be greater than zero.")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Timestamp is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "TakenAt must be a valid date and time.")]
        public DateTime TakenAt { get; set; }

        [Required(ErrorMessage = "Source is required.")]
        [StringLength(100, ErrorMessage = "Source cannot exceed 100 characters.")]
        public string Source { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        [Required]
        public AdherenceStatus Status { get; set; }
    }
}