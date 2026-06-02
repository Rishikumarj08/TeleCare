using System;
using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class TelemetryPointModel
    {
        [Key]
        public int TelemetryID { get; set; }

        [Required(ErrorMessage = "Device ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Device ID must be a positive integer.")]
        public int DeviceID { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient ID must be a positive integer.")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Metric name is required.")]
        [StringLength(100, ErrorMessage = "Metric name must be between 2 and 100 characters.", MinimumLength = 2)]
        public string MetricName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Value is required.")]
        public double Value { get; set; }

        [Required(ErrorMessage = "Unit is required.")]
        public string Unit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Timestamp is required.")]
        public DateTime Timestamp { get; set; }

        public DateTime IngestedAt { get; set; }
        public string? Source { get; set; }
        public bool ValidatedFlag { get; set; }
        public int Status { get; set; }
    }
}