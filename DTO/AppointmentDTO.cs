using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientReferenceNumber { get; set; }

        public DateTime AppointmentDateTime { get; set; }

        public string AppointmentType { get; set; }

        public string AppointmentMode { get; set; }

        public AppointmentStatusEnum AppointmentStatus { get; set; }
    }
}