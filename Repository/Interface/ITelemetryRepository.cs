using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface ITelemetryRepository
    {
        Task<TelemetryPointModel> createTelemetryRecordAsync(TelemetryPointModel telemetryPointModel);
        Task<TelemetryPointModel?> getTelemetryRecordByTelemetryIdAsync(int telemetryId);
        Task<List<TelemetryPointModel>> getFilteredTelemetryRecordsAsync(TelemetryQueryDto telemetryQueryDto);
    }
}