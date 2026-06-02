using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class ProgramModel
    {
        [Key]
        public int ProgramID { get; set; }

        [Required(ErrorMessage = "Program name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string ProgramName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        public ProgramStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}