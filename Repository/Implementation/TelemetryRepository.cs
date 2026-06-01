using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly AppDbContext context;

        public TelemetryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<TelemetryPointModel> createTelemetryRecordAsync(TelemetryPointModel telemetryPointModel)
        {
            await context.TelemetryPoints.AddAsync(telemetryPointModel);
            await context.SaveChangesAsync();
            return telemetryPointModel;
        }

        public async Task<TelemetryPointModel?> getTelemetryRecordByTelemetryIdAsync(int telemetryId)
        {
            return await context.TelemetryPoints.FirstOrDefaultAsync(x => x.TelemetryID == telemetryId);
        }

        public async Task<List<TelemetryPointModel>> getFilteredTelemetryRecordsAsync(TelemetryQueryDto telemetryQueryDto)
        {
            var query = context.TelemetryPoints.AsQueryable();

            if (!string.IsNullOrWhiteSpace(telemetryQueryDto.SearchText))
                query = query.Where(x => x.MetricName.Contains(telemetryQueryDto.SearchText) || x.Unit.Contains(telemetryQueryDto.SearchText));

            if (telemetryQueryDto.PatientID.HasValue)
                query = query.Where(x => x.PatientID == telemetryQueryDto.PatientID.Value);

            if (telemetryQueryDto.DeviceID.HasValue)
                query = query.Where(x => x.DeviceID == telemetryQueryDto.DeviceID.Value);

            if (telemetryQueryDto.Source.HasValue)
            {
                var sourceStr = telemetryQueryDto.Source.Value.ToString();
                query = query.Where(x => x.Source == sourceStr);
            }

            return await query.ToListAsync();
        }
    }
}