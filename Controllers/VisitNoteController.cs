using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;

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
        public async Task<IActionResult> createVisitNoteRecord([FromBody] VisitNoteDto dto)
        {
            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }
            else
            {
                var result = await visitNoteService.createVisitNoteRecordAsync(dto);
                return Ok(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> getAllVisitNoteRecords()
        {
            var result = await visitNoteService.getAllVisitNoteRecordsAsync();
            return Ok(result);
        }

        [HttpGet("{visitNoteId}")]
        public async Task<IActionResult> getVisitNoteDetailsByVisitNoteId(int visitNoteId)
        {
            if (visitNoteId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidVisitNoteId);
            }
            else
            {
                var result = await visitNoteService.getVisitNoteDetailsByVisitNoteIdAsync(visitNoteId);

                if (result == null)
                {
                    return NotFound(ApplicationMessages.VisitNoteNotFound);
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpPut("{visitNoteId}")]
        public async Task<IActionResult> updateVisitNoteDetailsByVisitNoteId(int visitNoteId, [FromBody] VisitNoteDto dto)
        {
            if (visitNoteId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidVisitNoteId);
            }
            else if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }
            else
            {
                var result = await visitNoteService.updateVisitNoteDetailsByVisitNoteIdAsync(visitNoteId, dto);

                if (result == null)
                {
                    return NotFound(ApplicationMessages.VisitNoteNotFound);
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> getFilteredVisitNoteRecords([FromQuery] VisitNoteQueryDto query)
        {
            var result = await visitNoteService.getFilteredVisitNoteRecordsAsync(query);
            return Ok(result);
        }
    }
}
