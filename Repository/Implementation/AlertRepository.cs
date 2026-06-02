using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class AlertRepository : IAlertRepository
    {
        private readonly AppDbContext context;

        public AlertRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Alert?> createAlertRecordAsync(Alert alert)
        {
            await context.Alerts.AddAsync(alert);
            await context.SaveChangesAsync();
            return alert;
        }

        public async Task<List<Alert>> getAllAlertRecordsAsync()
        {
            return await context.Alerts.ToListAsync();
        }

        public async Task<Alert?> getAlertRecordByAlertIdAsync(int alertId)
        {
            return await context.Alerts
                .FirstOrDefaultAsync(x => x.Id == alertId);
        }

        public async Task<Alert?> updateAlertRecordByAlertIdAsync(Alert alert)
        {
            context.Alerts.Update(alert);
            await context.SaveChangesAsync();
            return alert;
        }
    }
}
