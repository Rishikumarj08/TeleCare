using System;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class EnrollmentResponseDto
    {
        public int EnrollID { get; set; }
        public int PatientID { get; set; }
        public int ProgramID { get; set; }
        public int EnrolledBy { get; set; }
        public DateTime EnrolledAt { get; set; }
        public required string ConsentDocumentURI { get; set; }
        public EnrollmentStatus Status { get; set; }
    }

    public class EnrollmentCreateDto
    {
        public int PatientID { get; set; }
        public int ProgramID { get; set; }
        public int EnrolledBy { get; set; }
        public required string ConsentDocumentURI { get; set; }
    }

    public class EnrollmentUpdateDto
    {
        public int EnrollID { get; set; }
        public required string ConsentDocumentURI { get; set; }
        public EnrollmentStatus Status { get; set; }
    }

    public class EnrollmentQueryDto
    {
        public int? PatientID { get; set; }
        public int? ProgramID { get; set; }
        public EnrollmentStatus? Status { get; set; }
    }
}