namespace TeleCare.Controllers;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Service.Interface;
 
[Route("api/admin/rules")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminRuleController : ControllerBase
{
    private readonly IRuleService _ruleService;
 
    public AdminRuleController(IRuleService ruleService)
    {
        _ruleService = ruleService;
    }
 
    /// <summary>
    /// Get all rules (RuleID is intentionally excluded from response DTO display)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRules()
    {
        var result = await _ruleService.GetAllRulesAsync();
        return Ok(result);
    }
 
    /// <summary>
    /// Search rules by RuleID, Name, Status, ActiveFrom, ActiveTo (any one attribute is sufficient)
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchRules([FromBody] SearchRuleDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        var result = await _ruleService.SearchRulesAsync(searchDto);
        return Ok(result);
    }
 
    [HttpPost]
    public async Task<IActionResult> CreateRule([FromBody] RuleCreateDto ruleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _ruleService.CreateRuleAsync(ruleDto);
        return Ok(AppConstants.RecordCreated);
    }
 
    [HttpPut("{ruleId}")]
    public async Task<IActionResult> UpdateRule(int ruleId, [FromBody] RuleCreateDto ruleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _ruleService.UpdateRuleAsync(ruleId, ruleDto);
        return Ok(AppConstants.RecordUpdated);
    }
 
    [HttpDelete("{ruleId}")]
    public async Task<IActionResult> DeleteRule(int ruleId)
    {
        await _ruleService.DeleteRuleAsync(ruleId);
        return Ok(AppConstants.RecordDeleted);
    }
}
 
 