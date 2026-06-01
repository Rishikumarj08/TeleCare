using System;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class DeviceResponseDto
    {
        public int DeviceID { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public int? AssignedToPatientID { get; set; }
        public DeviceStatus Status { get; set; }
        public DateTime ProvisionedAt { get; set; }
    }

    public class DeviceCreateDto
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public int? AssignedToPatientID { get; set; }
    }

    public class DeviceUpdateDto
    {
        public int DeviceID { get; set; }
        public string Model { get; set; } = string.Empty;
        public DeviceStatus Status { get; set; }
        public int? AssignedToPatientID { get; set; }
    }

    public class DeviceQueryDto
    {
        public string? SearchText { get; set; }
        public string? Model { get; set; }
        public DeviceStatus? Status { get; set; }
    }
}