namespace TeleCare.Controllers;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Service.Interface;
 
[Route("api/admin/charges")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminChargeController : ControllerBase
{
    private readonly IChargeService _chargeService;
 
    public AdminChargeController(IChargeService chargeService)
    {
        _chargeService = chargeService;
    }
 
    /// <summary>
    /// Get all charges
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllCharges()
    {
        var result = await _chargeService.GetAllChargesAsync();
        return Ok(result);
    }
 
    /// <summary>
    /// Search charges by ChargeID, PatientName, Date, Status (any one attribute is sufficient)
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchCharges([FromBody] SearchChargeDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        var result = await _chargeService.SearchChargesAsync(searchDto);
        return Ok(result);
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateCharge([FromBody] ChargeCreateDto chargeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _chargeService.CreateChargeAsync(chargeDto);
        return Ok(AppConstants.RecordCreated);
    }
 
    [HttpPut("{chargeId}")]
    public async Task<IActionResult> UpdateCharge(int chargeId, [FromBody] ChargeCreateDto chargeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _chargeService.UpdateChargeAsync(chargeId, chargeDto);
        return Ok(AppConstants.RecordUpdated);
    }
 
    [HttpDelete("{chargeId}")]
    public async Task<IActionResult> DeleteCharge(int chargeId)
    {
        await _chargeService.DeleteChargeAsync(chargeId);
        return Ok(AppConstants.RecordDeleted);
    }
}
 
 