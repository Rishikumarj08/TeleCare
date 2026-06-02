using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientReferenceNumber { get; set; }

        public DateTime AppointmentDateTime { get; set; }

        public required string AppointmentType { get; set; }

        public required string AppointmentMode { get; set; }

        public AppointmentStatusEnum AppointmentStatus { get; set; }
    }
}