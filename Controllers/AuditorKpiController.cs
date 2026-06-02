namespace TeleCare.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Service.Interface;

[Route("api/auditor/kpis")]
[ApiController]
[Authorize(Roles = "Auditor")]
public class AuditorKpiController : ControllerBase
{
    private readonly IKpiService _kpiService;

    public AuditorKpiController(IKpiService kpiService)
    {
        _kpiService = kpiService;
    }

    /// <summary>
    /// Get all KPIs with auto-calculated CurrentValue and PerformanceIndicator
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllKpis()
    {
        var result = await _kpiService.GetAllKpisAsync();
        return Ok(result);
    }

    /// <summary>
    /// Search KPIs by Name, ReportingPeriod, PerformanceIndicator (any one is sufficient)
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchKpis([FromBody] SearchKpiDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _kpiService.SearchKpisAsync(searchDto);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateKpi([FromBody] KpiCreateDto kpiDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _kpiService.CreateKpiAsync(kpiDto);
        return Ok(AppConstants.RecordCreated);
    }

    [HttpPut("{kpiId}")]
    public async Task<IActionResult> UpdateKpi(int kpiId, [FromBody] KpiCreateDto kpiDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _kpiService.UpdateKpiAsync(kpiId, kpiDto);
        return Ok(AppConstants.RecordUpdated);
    }

    [HttpDelete("{kpiId}")]
    public async Task<IActionResult> DeleteKpi(int kpiId)
    {
        await _kpiService.DeleteKpiAsync(kpiId);
        return Ok(AppConstants.RecordDeleted);
    }
}
