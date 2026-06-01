using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface ITelemetryService
    {
        Task<TelemetryResponseDto> createTelemetryRecordAsync(TelemetryCreateDto telemetryCreateDto);
        Task<TelemetryResponseDto?> getTelemetryDetailsByTelemetryIdAsync(int telemetryId);
        Task<List<TelemetryResponseDto>> getFilteredTelemetryRecordsAsync(TelemetryQueryDto telemetryQueryDto);
    }
}