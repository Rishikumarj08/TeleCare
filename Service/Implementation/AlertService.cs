using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository repository;
        private readonly IAuditLogService _auditLogService;

        public AlertService(IAlertRepository repository, IAuditLogService auditLogService)
        {
            this.repository = repository;
            _auditLogService = auditLogService;
        }

        public async Task<AlertDto?> createAlertRecordAsync(AlertDto dto)
        {
            var entity = new Alert
            {
                PatientReferenceNumber = dto.PatientReferenceNumber,
                AlertType = dto.AlertType,
                AlertSeverity = dto.AlertSeverity,
                Message = dto.Message,
                AlertStatus = dto.AlertStatus,
                CreatedOn = DateTime.Now
            };

            Validate(entity);

            var result = await repository.createAlertRecordAsync(entity);
            if (result != null)
            {
                dto.AlertId = result.Id;
            }

            await _auditLogService.LogAsync(entity.PatientReferenceNumber, "CREATE", "Alert", result?.Id,
                $"Alert '{dto.AlertType}' created for patient '{dto.PatientReferenceNumber}'. Severity: '{dto.AlertSeverity}'.");

            return dto;
        }

        public async Task<List<AlertDto>> getAllAlertRecordsAsync()
        {
            var data = await repository.getAllAlertRecordsAsync();

            return data.Select(x => new AlertDto
            {
                AlertId = x.Id,
                PatientReferenceNumber = x.PatientReferenceNumber,
                AlertType = x.AlertType,
                AlertSeverity = x.AlertSeverity,
                Message = x.Message,
                AlertStatus = x.AlertStatus
            }).ToList();
        }

        public async Task<AlertDto?> getAlertDetailsByAlertIdAsync(int id)
        {
            var entity = await repository.getAlertRecordByAlertIdAsync(id);

            return entity == null ? null : new AlertDto
            {
                AlertId = entity.Id,
                PatientReferenceNumber = entity.PatientReferenceNumber,
                AlertType = entity.AlertType,
                AlertSeverity = entity.AlertSeverity,
                Message = entity.Message,
                AlertStatus = entity.AlertStatus
            };
        }

        public async Task<AlertDto?> updateAlertDetailsByAlertIdAsync(int id, AlertDto dto)
        {
            var entity = await repository.getAlertRecordByAlertIdAsync(id);

            if (entity == null) return null;

            entity.AlertType = dto.AlertType;
            entity.AlertSeverity = dto.AlertSeverity;
            entity.Message = dto.Message;
            entity.AlertStatus = dto.AlertStatus;

            Validate(entity);

            var updated = await repository.updateAlertRecordByAlertIdAsync(entity);
            if (updated == null) return null;

            await _auditLogService.LogAsync(updated.PatientReferenceNumber, "UPDATE", "Alert", id,
                $"Alert '{id}' updated. Status: '{dto.AlertStatus}'.");

            return new AlertDto
            {
                AlertId = updated.Id,
                PatientReferenceNumber = updated.PatientReferenceNumber,
                AlertType = updated.AlertType,
                AlertSeverity = updated.AlertSeverity,
                Message = updated.Message,
                AlertStatus = updated.AlertStatus
            };
        }

        private void Validate(Alert entity)
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
