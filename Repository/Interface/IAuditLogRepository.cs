namespace TeleCare.Repository.Interface
{
    using TeleCare.DTO;
    using TeleCare.Model;

    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetAllAuditLogsAsync();
        Task<List<AuditLog>> SearchAuditLogsAsync(SearchAuditLogDto searchDto);
        Task AddAuditLogAsync(AuditLog auditLog);
    }
}
