using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IAppointmentService
    {
        Task<AppointmentResponseDto> createAppointmentAsync(AppointmentCreateDto dto);

        Task<List<AppointmentResponseDto>> getAllAppointmentsAsync();

        Task<AppointmentResponseDto?> getAppointmentByIdAsync(int appointmentId);

        Task<AppointmentResponseDto?> updateAppointmentAsync(int appointmentId, AppointmentCreateDto dto);

        Task<List<AppointmentResponseDto>> getFilteredAppointmentsAsync(AppointmentQueryDto queryDto);
    }
}