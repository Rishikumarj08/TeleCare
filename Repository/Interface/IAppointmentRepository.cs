using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment> createAppointmentAsync(Appointment appointment);

        Task<List<Appointment>> getAllAppointmentsAsync();

        Task<Appointment?> getAppointmentByIdAsync(int appointmentId);

        Task<Appointment> updateAppointmentAsync(Appointment appointment);

        Task<List<Appointment>> getFilteredAppointmentsAsync(AppointmentQueryDto queryDto);
    }
}