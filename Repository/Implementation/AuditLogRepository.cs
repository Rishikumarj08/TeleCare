namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.DTO;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;

    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLog>> GetAllAuditLogsAsync()
        {
            return await _context.AuditLogs
                .Include(a => a.PerformedBy)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task<List<AuditLog>> SearchAuditLogsAsync(SearchAuditLogDto searchDto)
        {
            var query = _context.AuditLogs
                .Include(a => a.PerformedBy)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.Action))
                query = query.Where(a => a.Action.ToLower() == searchDto.Action.Trim().ToLower());

            if (!string.IsNullOrWhiteSpace(searchDto.ResourceType))
                query = query.Where(a => a.ResourceType.ToLower() == searchDto.ResourceType.Trim().ToLower());

            if (!string.IsNullOrWhiteSpace(searchDto.PerformedBy))
                query = query.Where(a => a.PerformedBy != null &&
                    a.PerformedBy.Name.Contains(searchDto.PerformedBy));

            if (searchDto.FromDate.HasValue)
                query = query.Where(a => a.Timestamp >= searchDto.FromDate.Value);

            if (searchDto.ToDate.HasValue)
                query = query.Where(a => a.Timestamp <= searchDto.ToDate.Value);

            return await query
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }

        public async Task AddAuditLogAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
