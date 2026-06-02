using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class CarePlan
    {
        [Key]
        public int CarePlanID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PatientID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProgramID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string PlanName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public CarePlanStatus Status { get; set; } = CarePlanStatus.Active;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}