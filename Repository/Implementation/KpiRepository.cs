namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;

    public class KpiRepository : IKpiRepository
    {
        private readonly AppDbContext _context;

        public KpiRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<KPI>> GetAllKpisAsync()
        {
            return await _context.KPIs.ToListAsync();
        }

        public async Task<KPI?> GetKpiByIdAsync(int kpiId)
        {
            return await _context.KPIs.FirstOrDefaultAsync(k => k.KPIID == kpiId);
        }

        public async Task AddKpiAsync(KPI kpi)
        {
            await _context.KPIs.AddAsync(kpi);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateKpiAsync(KPI kpi)
        {
            _context.KPIs.Update(kpi);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteKpiAsync(KPI kpi)
        {
            _context.KPIs.Remove(kpi);
            await _context.SaveChangesAsync();
        }
    }
}
