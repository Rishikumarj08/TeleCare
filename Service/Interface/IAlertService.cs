using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IAlertService
    {
        Task<AlertResponseDto> createAlertAsync(AlertCreateDto dto);

        Task<List<AlertResponseDto>> getAllAlertsAsync();

        Task<AlertResponseDto?> getAlertByIdAsync(int alertId);

        Task<AlertResponseDto?> updateAlertAsync(int alertId, AlertCreateDto dto);
    }
}