using System;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class TelemetryResponseDto
    {
        public int TelemetryID { get; set; }
        public int DeviceID { get; set; }
        public int PatientID { get; set; }
        public string MetricName { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public TelemetrySource Source { get; set; }
        public DateTime IngestedAt { get; set; }
        public bool ValidatedFlag { get; set; }
    }

    public class TelemetryCreateDto
    {
        public int DeviceID { get; set; }
        public int PatientID { get; set; }
        public string MetricName { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public TelemetrySource Source { get; set; }
    }

    public class TelemetryQueryDto
    {
        public string? SearchText { get; set; }
        public int? PatientID { get; set; }
        public int? DeviceID { get; set; }
        public TelemetrySource? Source { get; set; }
    }
}