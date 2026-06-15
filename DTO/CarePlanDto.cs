using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class CarePlanCreateDTO
    {
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
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class CarePlanUpdateDTO
    {
        [Required]
        public int CarePlanID { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string PlanName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public CarePlanStatus Status { get; set; }
    }

    public class CarePlanResponseDTO
    {
        public string PlanName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CarePlanStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CarePlanSearchDTO
    {
        public int? PatientID { get; set; }
        public int? ProgramID { get; set; }
        public CarePlanStatus? Status { get; set; }

        [StringLength(100)]
        public string? SearchText { get; set; }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}
