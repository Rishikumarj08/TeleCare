namespace TeleCare.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;

[Route("api/auditor/patientvisits")]
[ApiController]
[Authorize(Roles = "Auditor")]
public class AuditorPatientVisitController : ControllerBase
{
    private readonly IAuditorVisitNoteService _visitNoteService;

    public AuditorPatientVisitController(IAuditorVisitNoteService visitNoteService)
    {
        _visitNoteService = visitNoteService;
    }

    /// <summary>
    /// Get all patient visits with PatientName joined from Patients table
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllPatientVisits()
    {
        var result = await _visitNoteService.GetAllVisitNotesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Search patient visits by PatientName, VisitNoteStatus, SearchText (any one is sufficient)
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchPatientVisits([FromBody] SearchVisitNoteDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _visitNoteService.SearchVisitNotesAsync(searchDto);
        return Ok(result);
    }
}
