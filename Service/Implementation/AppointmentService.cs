using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository repository;
        private readonly IAuditLogService _auditLogService;

        public AppointmentService(IAppointmentRepository repository, IAuditLogService auditLogService)
        {
            this.repository = repository;
            _auditLogService = auditLogService;
        }

        public async Task<AppointmentResponseDto> createAppointmentAsync(AppointmentCreateDto dto)
        {
            var entity = new Appointment
            {
                PatientID = dto.PatientID,
                ClinicianID = dto.ClinicianID,
                ScheduledAt = dto.ScheduledAt,
                DurationMinutes = dto.DurationMinutes,
                Mode = dto.Mode,
                LocationURI = dto.LocationURI,
                Status = dto.Status,
                CreatedAt = DateTime.UtcNow
            };

            Validate(entity);

            var result = await repository.createAppointmentAsync(entity);

            await _auditLogService.LogAsync(entity.PatientID, "CREATE", "Appointment", result.AppID,
                $"Appointment created for Patient '{dto.PatientID}' at '{dto.ScheduledAt}'.");

            return MapToResponseDto(result);
        }

        public async Task<List<AppointmentResponseDto>> getAllAppointmentsAsync()
        {
            var data = await repository.getAllAppointmentsAsync();

            return data.Select(x => MapToResponseDto(x)).ToList();
        }

        public async Task<AppointmentResponseDto?> getAppointmentByIdAsync(int appointmentId)
        {
            var entity = await repository.getAppointmentByIdAsync(appointmentId);

            if (entity == null) return null;

            return MapToResponseDto(entity);
        }

        public async Task<AppointmentResponseDto?> updateAppointmentAsync(int appointmentId, AppointmentCreateDto dto)
        {
            var entity = await repository.getAppointmentByIdAsync(appointmentId);

            if (entity == null) return null;

            entity.PatientID = dto.PatientID;
            entity.ClinicianID = dto.ClinicianID;
            entity.ScheduledAt = dto.ScheduledAt;
            entity.DurationMinutes = dto.DurationMinutes;
            entity.Mode = dto.Mode;
            entity.LocationURI = dto.LocationURI;
            entity.Status = dto.Status;

            Validate(entity);

            var updated = await repository.updateAppointmentAsync(entity);

            await _auditLogService.LogAsync(entity.PatientID, "UPDATE", "Appointment", appointmentId,
                $"Appointment '{appointmentId}' updated. Status: '{dto.Status}'.");

            return MapToResponseDto(updated);
        }

        public async Task<List<AppointmentResponseDto>> getFilteredAppointmentsAsync(AppointmentQueryDto query)
        {
            var data = await repository.getFilteredAppointmentsAsync(query);

            return data.Select(x => MapToResponseDto(x)).ToList();
        }

        private static AppointmentResponseDto MapToResponseDto(Appointment entity)
        {
            return new AppointmentResponseDto
            {
                AppID = entity.AppID,
                PatientID = entity.PatientID,
                ClinicianID = entity.ClinicianID,
                ScheduledAt = entity.ScheduledAt,
                DurationMinutes = entity.DurationMinutes,
                Mode = entity.Mode,
                LocationURI = entity.LocationURI,
                Status = entity.Status,
                CreatedAt = entity.CreatedAt
            };
        }

        private void Validate(Appointment entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new Exception(results.First().ErrorMessage);
            }
        }
    }
}