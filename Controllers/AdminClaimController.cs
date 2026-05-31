namespace TeleCare.Controllers;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;
 
[Route("api/admin/claims")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminClaimController : ControllerBase
{
    private readonly IClaimService _claimService;
    private readonly IPayerRepository _payerRepository;
 
    public AdminClaimController(IClaimService claimService, IPayerRepository payerRepository)
    {
        _claimService = claimService;
        _payerRepository = payerRepository;
    }
 
    /// <summary>
    /// Get all claims
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllClaims()
    {
        var result = await _claimService.GetAllClaimsAsync();
        return Ok(result);
    }
 
    /// <summary>
    /// Search claims by ClaimID, PatientName, PayerName, Status, SubmittedAt (any one attribute is sufficient)
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchClaims([FromBody] SearchClaimDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        var result = await _claimService.SearchClaimsAsync(searchDto);
        return Ok(result);
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateClaim([FromBody] ClaimCreateDto claimDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _claimService.CreateClaimAsync(claimDto);
        return Ok(AppConstants.RecordCreated);
    }
 
    [HttpPut("{claimId}")]
    public async Task<IActionResult> UpdateClaim(int claimId, [FromBody] ClaimCreateDto claimDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _claimService.UpdateClaimAsync(claimId, claimDto);
        return Ok(AppConstants.RecordUpdated);
    }
 
    [HttpDelete("{claimId}")]
    public async Task<IActionResult> DeleteClaim(int claimId)
    {
        await _claimService.DeleteClaimAsync(claimId);
        return Ok(AppConstants.RecordDeleted);
    }
 
    /// <summary>
    /// Lookup endpoint — returns payer list for the claim add/edit dropdown in the admin interface
    /// </summary>
    [HttpGet("payers")]
    public async Task<IActionResult> GetPayers()
    {
        var payers = await _payerRepository.GetAllPayersAsync();
        var result = payers.Select(p => new { p.PayerID, p.PayerName });
        return Ok(result);
    }
}
 
 