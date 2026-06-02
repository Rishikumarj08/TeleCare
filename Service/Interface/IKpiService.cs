namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;

    public interface IKpiService
    {
        Task<List<KpiResponseDto>> GetAllKpisAsync();
        Task<KpiResponseDto> GetKpiByIdAsync(int kpiId);
        Task<List<KpiResponseDto>> SearchKpisAsync(SearchKpiDto searchDto);
        Task CreateKpiAsync(KpiCreateDto kpiDto);
        Task UpdateKpiAsync(int kpiId, KpiCreateDto kpiDto);
        Task DeleteKpiAsync(int kpiId);
    }
}
