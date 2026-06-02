namespace TeleCare.Service.Implementation
{
    using TeleCare.Constants;
    using TeleCare.DTO;
    using TeleCare.Exceptions;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
    using TeleCare.Service.Interface;

    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<List<AuditLogResponseDto>> GetAllAuditLogsAsync()
        {
            var logs = await _auditLogRepository.GetAllAuditLogsAsync();
            if (logs == null || logs.Count == 0)
                throw new NotFoundException(AppConstants.NoAuditLogsFound);
            return logs.Select(Map).ToList();
        }

        public async Task<List<AuditLogResponseDto>> SearchAuditLogsAsync(SearchAuditLogDto searchDto)
        {
            var logs = await _auditLogRepository.SearchAuditLogsAsync(searchDto);
            if (logs == null || logs.Count == 0)
                throw new NotFoundException(AppConstants.NoAuditLogsFound);
            return logs.Select(Map).ToList();
        }

        public async Task LogAsync(int userId, string action, string resourceType, int? resourceId, string? details)
        {
            var auditLog = new AuditLog
            {
                UserID = userId,
                Action = action,
                ResourceType = resourceType,
                ResourceID = resourceId,
                DetailsJSON = details,
                Timestamp = DateTime.UtcNow
            };

            await _auditLogRepository.AddAuditLogAsync(auditLog);
        }

        private static AuditLogResponseDto Map(AuditLog log) => new()
        {
            Action = log.Action,
            ResourceType = log.ResourceType,
            PerformedBy = log.PerformedBy?.Name ?? string.Empty,
            DetailsJSON = log.DetailsJSON,
            Timestamp = log.Timestamp
        };
    }
}
