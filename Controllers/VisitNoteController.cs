using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using Microsoft.AspNetCore.Authorization;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/visitnote")]
    public class VisitNoteController : ControllerBase
    {
        private readonly IVisitNoteService visitNoteService;

        public VisitNoteController(IVisitNoteService visitNoteService)
        {
            this.visitNoteService = visitNoteService;
        }

        [HttpPost]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> createVisitNote([FromBody] VisitNoteCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }

            var result = await visitNoteService.createVisitNoteAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> getAllVisitNotes()
        {
            var result = await visitNoteService.getAllVisitNotesAsync();
            return Ok(result);
        }

        [HttpGet("{visitNoteId}")]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> getVisitNoteById(int visitNoteId)
        {
            if (visitNoteId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidVisitNoteId);
            }

            var result = await visitNoteService.getVisitNoteByIdAsync(visitNoteId);

            if (result == null)
            {
                return NotFound(ApplicationMessages.VisitNoteNotFound);
            }

            return Ok(result);
        }

        [HttpPut("{visitNoteId}")]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> updateVisitNote(int visitNoteId, [FromBody] VisitNoteCreateDto dto)
        {
            if (visitNoteId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidVisitNoteId);
            }

            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }

            var result = await visitNoteService.updateVisitNoteAsync(visitNoteId, dto);

            if (result == null)
            {
                return NotFound(ApplicationMessages.VisitNoteNotFound);
            }

            return Ok(result);
        }

        [HttpGet("filter")]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> getFilteredVisitNotes([FromQuery] VisitNoteQueryDto query)
        {
            var result = await visitNoteService.getFilteredVisitNotesAsync(query);
            return Ok(result);
        }
    }
}