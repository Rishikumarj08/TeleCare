namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;

    public interface IAuditLogService
    {
        Task<List<AuditLogResponseDto>> GetAllAuditLogsAsync();
        Task<List<AuditLogResponseDto>> SearchAuditLogsAsync(SearchAuditLogDto searchDto);
        Task LogAsync(int userId, string action, string resourceType, int? resourceId, string? details);
    }
}
