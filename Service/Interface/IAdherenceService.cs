using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IAdherenceService
    {
        Task<AdherenceResponseDto> createAdherenceRecordAsync(AdherenceCreateDto adherenceCreateDto);
        Task<AdherenceResponseDto?> getAdherenceDetailsByAdhIDAsync(int adhId);
        Task<AdherenceResponseDto?> updateAdherenceDetailsByAdhIDAsync(int adhId, AdherenceUpdateDto adherenceUpdateDto);
        Task<List<AdherenceResponseDto>> getFilteredAdherenceRecordsAsync(AdherenceQueryDto adherenceQueryDto);
    }
}