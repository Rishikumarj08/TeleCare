namespace TeleCare.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;

[Route("api/auditor/auditlogs")]
[ApiController]
[Authorize(Roles = "Auditor")]
public class AuditorAuditLogController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditorAuditLogController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    /// <summary>
    /// Get all audit logs — Auditor read-only access
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAuditLogs()
    {
        var result = await _auditLogService.GetAllAuditLogsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Search audit logs by Action, ResourceType, PerformedBy, FromDate, ToDate (any one attribute is sufficient)
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchAuditLogs([FromBody] SearchAuditLogDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _auditLogService.SearchAuditLogsAsync(searchDto);
        return Ok(result);
    }
}
