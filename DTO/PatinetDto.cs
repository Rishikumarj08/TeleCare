using System;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    // Used for GET responses
    public class PatientResponseDto
    {
        public int PatientID { get; set; }
        public int UserID { get; set; }
        public string MRN { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ContactInfoJSON { get; set; }
        public string EmergencyContactJSON { get; set; }
        public string PrimaryLanguage { get; set; }
        public bool ConsentStatus { get; set; }
        public string EnrolledProgramsJSON { get; set; }
        public PatientStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Used for POST (Create) - PatientID is NOT here
    public class PatientCreateDto
    {
        public int UserID { get; set; }
        public string MRN { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ContactInfoJSON { get; set; }
        public string PrimaryLanguage { get; set; }
        public string EmergencyContactJSON { get; set; }
        public bool ConsentStatus { get; set; }
        public string EnrolledProgramsJSON { get; set; }
    }

    // Used for PUT (Update)
    public class PatientUpdateDto
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInfoJSON { get; set; }
        public string EmergencyContactJSON { get; set; }
    }

    public class PatientQueryDto
    {
        public string? SearchText { get; set; }
        public PatientStatus? Status { get; set; }
        public int? UserID { get; set; }
    }
}