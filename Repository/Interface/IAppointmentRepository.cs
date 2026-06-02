using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> createAppointmentRecordAsync(Appointment appointment);

        Task<List<Appointment>> getAllAppointmentRecordsAsync();

        Task<Appointment?> getAppointmentRecordByAppointmentIdAsync(int appointmentId);

        Task<Appointment?> updateAppointmentRecordByAppointmentIdAsync(Appointment appointment);

        Task<List<Appointment>> getFilteredAppointmentRecordsAsync(AppointmentQueryDto queryDto);

    }
}
