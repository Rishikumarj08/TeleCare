using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> createAppointmentRecordAsync(AppointmentDto appointmentDto);

        Task<List<AppointmentDto>> getAllAppointmentRecordsAsync();

        Task<AppointmentDto> getAppointmentDetailsByAppointmentIdAsync(int appointmentId);

        Task<AppointmentDto> updateAppointmentDetailsByAppointmentIdAsync(int appointmentId, AppointmentDto appointmentDto);

        Task<List<AppointmentDto>> getFilteredAppointmentRecordsAsync(AppointmentQueryDto queryDto);
    }
}
