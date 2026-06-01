using TeleCare.Enum;

namespace TeleCare.DTO
{
    public class AppointmentQueryDto
    {
        public string? SearchText { get; set; }

        public AppointmentStatusEnum? AppointmentStatus { get; set; }

        public DateTime? AppointmentDate { get; set; }
    }
}
