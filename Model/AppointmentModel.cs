using System.ComponentModel.DataAnnotations;
using TeleCare.Enum;

namespace TeleCare.Model
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Patient reference number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Patient reference number must be greater than zero.")]
        public int PatientReferenceNumber { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        public DateTime AppointmentDateTime { get; set; }

        [Required(ErrorMessage = "Appointment type is required.")]
        public string AppointmentType { get; set; }

        [Required(ErrorMessage = "Appointment mode is required.")]
        public string AppointmentMode { get; set; }

        [Required(ErrorMessage = "Appointment status is required.")]
        public AppointmentStatusEnum AppointmentStatus { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}