using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enums;

namespace TeleCare.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PatientId { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Dose { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Frequency { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Route { get; set; } = string.Empty;

        [Required]
        public DateTime StartAt { get; set; }

        public DateTime? EndAt { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PrescribedBy { get; set; }

        public MedicationStatus Status { get; set; } = MedicationStatus.Active;
    }
}