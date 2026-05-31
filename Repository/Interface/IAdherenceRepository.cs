using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IAdherenceRepository
    {
        Task<AdherenceRecordModel> createAdherenceRecordAsync(AdherenceRecordModel adherenceRecordModel);
        Task<AdherenceRecordModel?> getAdherenceRecordByAdhIDAsync(int adhId);
        Task<AdherenceRecordModel> updateAdherenceRecordByAdhIDAsync(AdherenceRecordModel adherenceRecordModel);
        Task<List<AdherenceRecordModel>> getFilteredAdherenceRecordsAsync(AdherenceQueryDto adherenceQueryDto);
    }
}