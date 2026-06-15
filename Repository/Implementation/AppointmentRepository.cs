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

        public async Task<Appointment> createAppointmentAsync(Appointment appointment)
        {
            await context.Appointments.AddAsync(appointment);
            await context.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<Appointment>> getAllAppointmentsAsync()
        {
            return await context.Appointments.ToListAsync();
        }

        public async Task<Appointment?> getAppointmentByIdAsync(int appointmentId)
        {
            return await context.Appointments
                .FirstOrDefaultAsync(x => x.AppID == appointmentId);
        }

        public async Task<Appointment> updateAppointmentAsync(Appointment appointment)
        {
            context.Appointments.Update(appointment);
            await context.SaveChangesAsync();
            return appointment;
        }

        public async Task<List<Appointment>> getFilteredAppointmentsAsync(AppointmentQueryDto queryDto)
        {
            var query = context.Appointments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryDto.SearchText))
            {
                query = query.Where(x =>
                    x.Mode.Contains(queryDto.SearchText) ||
                    x.Status.Contains(queryDto.SearchText));
            }

            if (!string.IsNullOrWhiteSpace(queryDto.Status))
            {
                query = query.Where(x => x.Status == queryDto.Status);
            }

            if (queryDto.ScheduledAt.HasValue)
            {
                var date = queryDto.ScheduledAt.Value.Date;
                query = query.Where(x => x.ScheduledAt.Date == date);
            }

            return await query.ToListAsync();
        }
    }
}