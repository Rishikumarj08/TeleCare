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

        public async Task<AlertResponseDto> createAlertAsync(AlertCreateDto dto)
        {
            var entity = new Alert
            {
                PatientID = dto.PatientID,
                RuleID = dto.RuleID,
                TriggeredAt = dto.TriggeredAt,
                Severity = dto.Severity,
                AssignedToFK = dto.AssignedToFK,
                AcknowledgedAt = dto.AcknowledgedAt,
                ResolvedAt = dto.ResolvedAt,
                Status = dto.Status
            };

            Validate(entity);

            var result = await repository.createAlertAsync(entity);

            await _auditLogService.LogAsync(entity.PatientID, "CREATE", "Alert", result.AlertID,
                $"Alert created with Severity: '{dto.Severity}', Status: '{dto.Status}'.");

            return MapToResponseDto(result);
        }

        public async Task<List<AlertResponseDto>> getAllAlertsAsync()
        {
            var data = await repository.getAllAlertsAsync();

            return data.Select(x => MapToResponseDto(x)).ToList();
        }

        public async Task<AlertResponseDto?> getAlertByIdAsync(int alertId)
        {
            var entity = await repository.getAlertByIdAsync(alertId);

            if (entity == null) return null;

            return MapToResponseDto(entity);
        }

        public async Task<AlertResponseDto?> updateAlertAsync(int alertId, AlertCreateDto dto)
        {
            var entity = await repository.getAlertByIdAsync(alertId);

            if (entity == null) return null;

            entity.PatientID = dto.PatientID;
            entity.RuleID = dto.RuleID;
            entity.TriggeredAt = dto.TriggeredAt;
            entity.Severity = dto.Severity;
            entity.AssignedToFK = dto.AssignedToFK;
            entity.AcknowledgedAt = dto.AcknowledgedAt;
            entity.ResolvedAt = dto.ResolvedAt;
            entity.Status = dto.Status;

            Validate(entity);

            var updated = await repository.updateAlertAsync(entity);

            await _auditLogService.LogAsync(entity.PatientID, "UPDATE", "Alert", alertId,
                $"Alert '{alertId}' updated. Status: '{dto.Status}'.");

            return MapToResponseDto(updated);
        }

        private static AlertResponseDto MapToResponseDto(Alert entity)
        {
            return new AlertResponseDto
            {
                AlertID = entity.AlertID,
                PatientID = entity.PatientID,
                RuleID = entity.RuleID,
                TriggeredAt = entity.TriggeredAt,
                Severity = entity.Severity,
                AssignedToFK = entity.AssignedToFK,
                AcknowledgedAt = entity.AcknowledgedAt,
                ResolvedAt = entity.ResolvedAt,
                Status = entity.Status
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
