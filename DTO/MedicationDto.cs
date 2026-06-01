using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enums;

namespace TeleCare.Dto
{
    //  CREATE & UPDATE DTO (used in Controller & Service)
    public class MedicationRequestDto
    {
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

        public MedicationStatus Status { get; set; } = MedicationStatus.Active;
    }

    //  SEARCH / FILTER DTO (used in Controller, Repository, Service)
    public class MedicationSearchDto
    {
        public string? Keyword { get; set; }
        public MedicationStatus? Status { get; set; }
        public int? PatientId { get; set; }

        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    //  RESPONSE DTO (used in Service return)
    public class MedicationResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Dose { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;

        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public MedicationStatus Status { get; set; }
        public string StatusLabel { get; set; } = string.Empty;
    }
}