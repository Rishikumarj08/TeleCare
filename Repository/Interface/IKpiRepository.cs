namespace TeleCare.Repository.Interface
{
    using TeleCare.Model;

    public interface IKpiRepository
    {
        Task<List<KPI>> GetAllKpisAsync();
        Task<KPI?> GetKpiByIdAsync(int kpiId);
        Task AddKpiAsync(KPI kpi);
        Task UpdateKpiAsync(KPI kpi);
        Task DeleteKpiAsync(KPI kpi);
    }
}
