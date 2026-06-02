using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class DeviceModel
    {
        [Key]
        public int DeviceID { get; set; }

        [Required(ErrorMessage = "Serial number is required.")]
        [StringLength(100, ErrorMessage = "Serial number must be between 2 and 100 characters.", MinimumLength = 2)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model name is required.")]
        [StringLength(100, ErrorMessage = "Model name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Device type is required.")]
        [StringLength(50, ErrorMessage = "Device type must be between 2 and 50 characters.", MinimumLength = 2)]
        public string DeviceType { get; set; } = string.Empty;

        public int? AssignedToPatientID { get; set; }

        [Required]
        public DateTime ProvisionedAt { get; set; }

        [Required]
        public DeviceStatus Status { get; set; }
    }
}