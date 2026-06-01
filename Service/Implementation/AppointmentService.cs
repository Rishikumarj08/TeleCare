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

        public AppointmentService(IAppointmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<AppointmentDto> createAppointmentRecordAsync(AppointmentDto dto)
        {
            var entity = new Appointment
            {
                PatientReferenceNumber = dto.PatientReferenceNumber,
                AppointmentDateTime = dto.AppointmentDateTime,
                AppointmentType = dto.AppointmentType,
                AppointmentMode = dto.AppointmentMode,
                AppointmentStatus = dto.AppointmentStatus,
                CreatedOn = DateTime.Now
            };

            Validate(entity);

            var result = await repository.createAppointmentRecordAsync(entity);
            dto.AppointmentId = result.Id;

            return dto;
        }

        public async Task<List<AppointmentDto>> getAllAppointmentRecordsAsync()
        {
            var data = await repository.getAllAppointmentRecordsAsync();

            return data.Select(x => new AppointmentDto
            {
                AppointmentId = x.Id,
                PatientReferenceNumber = x.PatientReferenceNumber,
                AppointmentDateTime = x.AppointmentDateTime,
                AppointmentType = x.AppointmentType,
                AppointmentMode = x.AppointmentMode,
                AppointmentStatus = x.AppointmentStatus
            }).ToList();
        }

        public async Task<AppointmentDto> getAppointmentDetailsByAppointmentIdAsync(int id)
        {
            var entity = await repository.getAppointmentRecordByAppointmentIdAsync(id);

            return entity == null ? null : new AppointmentDto
            {
                AppointmentId = entity.Id,
                PatientReferenceNumber = entity.PatientReferenceNumber,
                AppointmentDateTime = entity.AppointmentDateTime,
                AppointmentType = entity.AppointmentType,
                AppointmentMode = entity.AppointmentMode,
                AppointmentStatus = entity.AppointmentStatus
            };
        }

        public async Task<AppointmentDto> updateAppointmentDetailsByAppointmentIdAsync(int id, AppointmentDto dto)
        {
            var entity = await repository.getAppointmentRecordByAppointmentIdAsync(id);

            if (entity == null) return null;

            entity.AppointmentDateTime = dto.AppointmentDateTime;
            entity.AppointmentType = dto.AppointmentType;
            entity.AppointmentMode = dto.AppointmentMode;
            entity.AppointmentStatus = dto.AppointmentStatus;

            Validate(entity);

            var updated = await repository.updateAppointmentRecordByAppointmentIdAsync(entity);

            return new AppointmentDto
            {
                AppointmentId = updated.Id,
                PatientReferenceNumber = updated.PatientReferenceNumber,
                AppointmentDateTime = updated.AppointmentDateTime,
                AppointmentType = updated.AppointmentType,
                AppointmentMode = updated.AppointmentMode,
                AppointmentStatus = updated.AppointmentStatus
            };
        }

        public async Task<List<AppointmentDto>> getFilteredAppointmentRecordsAsync(AppointmentQueryDto query)
        {
            var data = await repository.getFilteredAppointmentRecordsAsync(query);

            return data.Select(x => new AppointmentDto
            {
                AppointmentId = x.Id,
                PatientReferenceNumber = x.PatientReferenceNumber,
                AppointmentDateTime = x.AppointmentDateTime,
                AppointmentType = x.AppointmentType,
                AppointmentMode = x.AppointmentMode,
                AppointmentStatus = x.AppointmentStatus
            }).ToList();
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