using System;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class AdherenceResponseDto
    {
        public int AdhID { get; set; }
        public int MedID { get; set; }
        public int PatientID { get; set; }
        public DateTime TakenAt { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public AdherenceStatus Status { get; set; }
    }

    public class AdherenceCreateDto
    {
        public int MedID { get; set; }
        public int PatientID { get; set; }
        public DateTime TakenAt { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public AdherenceStatus Status { get; set; }
    }

    public class AdherenceUpdateDto
    {
        public int AdhID { get; set; }
        public DateTime TakenAt { get; set; }
        public string Notes { get; set; } = string.Empty;
        public AdherenceStatus Status { get; set; }
    }

    public class AdherenceQueryDto
    {
        public string? SearchText { get; set; }
        public int? PatientID { get; set; }
        public int? MedID { get; set; }
        public AdherenceStatus? Status { get; set; }
    }
}