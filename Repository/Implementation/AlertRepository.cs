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

        public async Task<Alert> createAlertAsync(Alert alert)
        {
            await context.Alerts.AddAsync(alert);
            await context.SaveChangesAsync();
            return alert;
        }

        public async Task<List<Alert>> getAllAlertsAsync()
        {
            return await context.Alerts.ToListAsync();
        }

        public async Task<Alert?> getAlertByIdAsync(int alertId)
        {
            return await context.Alerts
                .FirstOrDefaultAsync(x => x.AlertID == alertId);
        }

        public async Task<Alert> updateAlertAsync(Alert alert)
        {
            context.Alerts.Update(alert);
            await context.SaveChangesAsync();
            return alert;
        }
    }
}
