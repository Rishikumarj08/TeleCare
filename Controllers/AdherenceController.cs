using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/adherence")]
    public class AdherenceController : ControllerBase
    {
        private readonly IAdherenceService adherenceService;

        public AdherenceController(IAdherenceService adherenceService)
        {
            this.adherenceService = adherenceService;
        }

        [HttpPost]
        public async Task<IActionResult> createAdherenceRecord([FromBody] AdherenceCreateDto adherenceCreateDtodto)
        {
            if (adherenceCreateDtodto == null) return BadRequest(AdherenceConstants.RequestBodyNull);
            
            var result = await adherenceService.createAdherenceRecordAsync(adherenceCreateDtodto);
            return Ok(result);
        }

        [HttpGet("{adherenceId}")]
        public async Task<IActionResult> getAdherenceDetailsByAdhID(int adherenceId)
        {
            if (adherenceId <= 0) return BadRequest(AdherenceConstants.InvalidAdherenceId);
            
            var result = await adherenceService.getAdherenceDetailsByAdhIDAsync(adherenceId);
            if (result == null) return NotFound(AdherenceConstants.RecordNotFound);
            
            return Ok(result);
        }

        [HttpPut("{adherenceId}")]
        public async Task<IActionResult> updateAdherenceDetailsByAdhID(int adherenceId, [FromBody] AdherenceUpdateDto adherenceUpdateDtodto)
        {
            if (adherenceId <= 0) 
                return BadRequest(AdherenceConstants.InvalidAdherenceId);

            var result = await adherenceService.updateAdherenceDetailsByAdhIDAsync(adherenceId, adherenceUpdateDtodto);
            if (result == null) return NotFound(AdherenceConstants.RecordNotFound);
            
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> getFilteredAdherenceRecords([FromQuery] AdherenceQueryDto adherenceQueryDtodto)
        {
            return Ok(await adherenceService.getFilteredAdherenceRecordsAsync(adherenceQueryDtodto));
        }
    }
}