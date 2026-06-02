using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class ProgramCreateDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string ProgramName { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public ProgramStatus Status { get; set; }
    }

    public class ProgramUpdateDTO
    {
        [Required]
        public int ProgramID { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string ProgramName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public ProgramStatus Status { get; set; }
    }

    public class ProgramResponseDTO
    {
        public int ProgramID { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ProgramSearchDTO
    {
        public int? ProgramID { get; set; }
        public int? PatientID { get; set; }
        public ProgramStatus? Status { get; set; }

        [StringLength(100)]
        public string? SearchText { get; set; }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}