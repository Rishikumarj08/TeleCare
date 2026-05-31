using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext context;

        public AppointmentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Appointment> createAppointmentRecordAsync(Appointment appointment)
        {
            await context.Appointments.AddAsync(appointment);
            await context.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<Appointment>> getAllAppointmentRecordsAsync()
        {
            return await context.Appointments.ToListAsync();
        }

        public async Task<Appointment> getAppointmentRecordByAppointmentIdAsync(int appointmentId)
        {
            return await context.Appointments
                .FirstOrDefaultAsync(x => x.Id == appointmentId);
        }

        public async Task<Appointment> updateAppointmentRecordByAppointmentIdAsync(Appointment appointment)
        {
            context.Appointments.Update(appointment);
            await context.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<Appointment>> getFilteredAppointmentRecordsAsync(AppointmentQueryDto queryDto)
        {
            var query = context.Appointments.AsQueryable();

            // CONDITION 1: Search by AppointmentType or AppointmentMode
            if (!string.IsNullOrWhiteSpace(queryDto.SearchText))
            {
                query = query.Where(x =>
                    x.AppointmentType.Contains(queryDto.SearchText) ||
                    x.AppointmentMode.Contains(queryDto.SearchText));
            }

            // CONDITION 2: Filter by Status
            if (queryDto.AppointmentStatus.HasValue)
            {
                query = query.Where(x => x.AppointmentStatus == queryDto.AppointmentStatus.Value);
            }

            // CONDITION 3: Filter by Date (only date part)
            if (queryDto.AppointmentDate.HasValue)
            {
                var date = queryDto.AppointmentDate.Value.Date;

                query = query.Where(x => x.AppointmentDateTime.Date == date);
            }

            return await query.ToListAsync();
        }

    }
}
