using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IAlertRepository
    {
        Task<Alert> createAlertAsync(Alert alert);

        Task<List<Alert>> getAllAlertsAsync();

        Task<Alert?> getAlertByIdAsync(int alertId);

        Task<Alert> updateAlertAsync(Alert alert);
    }
}