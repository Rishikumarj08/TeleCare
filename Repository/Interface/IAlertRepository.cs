using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IAlertRepository
    {
        Task<Alert?> createAlertRecordAsync(Alert alert);

        Task<List<Alert>> getAllAlertRecordsAsync();

        Task<Alert?> getAlertRecordByAlertIdAsync(int alertId);

        Task<Alert?> updateAlertRecordByAlertIdAsync(Alert alert);
    }
}