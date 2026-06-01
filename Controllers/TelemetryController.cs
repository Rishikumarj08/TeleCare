using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/telemetry")]
    public class TelemetryController : ControllerBase
    {
        private readonly ITelemetryService telemetryService;

        public TelemetryController(ITelemetryService telemetryService)
        {
            this.telemetryService = telemetryService;
        }

        [HttpPost]
        public async Task<IActionResult> createTelemetryRecord([FromBody] TelemetryCreateDto telemetryCreateDto)
        {
            if (telemetryCreateDto == null) return BadRequest(TelemetryConstants.RequestBodyNull);

            var result = await telemetryService.createTelemetryRecordAsync(telemetryCreateDto);
            return Ok(result);
        }

        [HttpGet("{telemetryId}")]
        public async Task<IActionResult> getTelemetryDetailsByTelemetryId(int telemetryId)
        {
            if (telemetryId <= 0) return BadRequest(TelemetryConstants.InvalidTelemetryId);

            var result = await telemetryService.getTelemetryDetailsByTelemetryIdAsync(telemetryId);
            if (result == null) return NotFound(TelemetryConstants.TelemetryNotFound);

            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> getFilteredTelemetryRecords([FromQuery] TelemetryQueryDto telemetryQueryDto)
        {
            return Ok(await telemetryService.getFilteredTelemetryRecordsAsync(telemetryQueryDto));
        }
    }
}