using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class TelemetryService : ITelemetryService
    {
        private readonly ITelemetryRepository repository;
        private readonly IAuditLogService _auditLogService;

        public TelemetryService(ITelemetryRepository repository, IAuditLogService auditLogService)
        {
            this.repository = repository;
            _auditLogService = auditLogService;
        }

        public async Task<TelemetryResponseDto> createTelemetryRecordAsync(TelemetryCreateDto telemetryCreateDto)
        {
            var entity = new TelemetryPointModel
            {
                DeviceID = telemetryCreateDto.DeviceID,
                PatientID = telemetryCreateDto.PatientID,
                MetricName = telemetryCreateDto.MetricName,
                Value = telemetryCreateDto.Value,
                Unit = telemetryCreateDto.Unit,
                Timestamp = telemetryCreateDto.Timestamp,
                Source = telemetryCreateDto.Source.ToString(),
                IngestedAt = DateTime.UtcNow,
                ValidatedFlag = true,
                Status = 1
            };

            Validate(entity);

            var result = await repository.createTelemetryRecordAsync(entity);
            await _auditLogService.LogAsync(entity.PatientID, "CREATE", "Telemetry", result.TelemetryID,
                $"Telemetry '{result.MetricName}' with value '{result.Value} {result.Unit}' recorded for patient '{result.PatientID}'.");
            return MapToDto(result);
        }

        public async Task<TelemetryResponseDto?> getTelemetryDetailsByTelemetryIdAsync(int telemetryId)
        {
            var entity = await repository.getTelemetryRecordByTelemetryIdAsync(telemetryId);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<List<TelemetryResponseDto>> getFilteredTelemetryRecordsAsync(TelemetryQueryDto telemetryQueryDto)
        {
            var data = await repository.getFilteredTelemetryRecordsAsync(telemetryQueryDto);
            return data.Select(MapToDto).ToList();
        }

        private void Validate(TelemetryPointModel entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new ArgumentException(results.First().ErrorMessage);
            }
        }

        private TelemetryResponseDto MapToDto(TelemetryPointModel telemetryPointModel) => new TelemetryResponseDto
        {
            TelemetryID = telemetryPointModel.TelemetryID,
            DeviceID = telemetryPointModel.DeviceID,
            PatientID = telemetryPointModel.PatientID,
            MetricName = telemetryPointModel.MetricName,
            Value = telemetryPointModel.Value,
            Unit = telemetryPointModel.Unit,
            Timestamp = telemetryPointModel.Timestamp,
            IngestedAt = telemetryPointModel.IngestedAt,
            ValidatedFlag = telemetryPointModel.ValidatedFlag,
            Source = System.Enum.TryParse<Enum.TelemetrySource>(telemetryPointModel.Source, out var src) ? src : Enum.TelemetrySource.Manual
        };
    }
}
