using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IAlertService
    {
        Task<AlertDto> createAlertRecordAsync(AlertDto alertDto);

        Task<List<AlertDto>> getAllAlertRecordsAsync();

        Task<AlertDto> getAlertDetailsByAlertIdAsync(int alertId);

        Task<AlertDto> updateAlertDetailsByAlertIdAsync(int alertId, AlertDto alertDto);
    }
}
