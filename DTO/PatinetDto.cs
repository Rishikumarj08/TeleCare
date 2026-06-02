using System;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    // Used for GET responses
    public class PatientResponseDto
    {
        public int PatientID { get; set; }
        public int UserID { get; set; }
        public required string MRN { get; set; }
        public required string Name { get; set; }
        public DateTime DOB { get; set; }
        public required string Gender { get; set; }
        public required string Address { get; set; }
        public required string ContactInfoJSON { get; set; }
        public required string EmergencyContactJSON { get; set; }
        public required string PrimaryLanguage { get; set; }
        public bool ConsentStatus { get; set; }
        public required string EnrolledProgramsJSON { get; set; }
        public PatientStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Used for POST (Create) - PatientID is NOT here
    public class PatientCreateDto
    {
        public int UserID { get; set; }
        public required string MRN { get; set; }
        public required string Name { get; set; }
        public DateTime DOB { get; set; }
        public required string Gender { get; set; }
        public required string Address { get; set; }
        public required string ContactInfoJSON { get; set; }
        public required string PrimaryLanguage { get; set; }
        public required string EmergencyContactJSON { get; set; }
        public bool ConsentStatus { get; set; }
        public required string EnrolledProgramsJSON { get; set; }
    }

    // Used for PUT (Update)
    public class PatientUpdateDto
    {
        public int PatientID { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string ContactInfoJSON { get; set; }
        public required string EmergencyContactJSON { get; set; }
    }

    public class PatientQueryDto
    {
        public string? SearchText { get; set; }
        public PatientStatus? Status { get; set; }
        public int? UserID { get; set; }
    }
}