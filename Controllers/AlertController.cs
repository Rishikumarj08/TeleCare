using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/alert")]
    public class AlertController : ControllerBase
    {
        private readonly IAlertService alertService;

        public AlertController(IAlertService alertService)
        {
            this.alertService = alertService;
        }

        [HttpPost]
        public async Task<IActionResult> createAlertRecord([FromBody] AlertDto alertDto)
        {
            if (alertDto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }
            else
            {
                var result = await alertService.createAlertRecordAsync(alertDto);
                return Ok(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> getAllAlertRecords()
        {
            var result = await alertService.getAllAlertRecordsAsync();
            return Ok(result);
        }

        [HttpGet("{alertId}")]
        public async Task<IActionResult> getAlertDetailsByAlertId(int alertId)
        {
            if (alertId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAlertId);
            }
            else
            {
                var result = await alertService.getAlertDetailsByAlertIdAsync(alertId);

                if (result == null)
                {
                    return NotFound(ApplicationMessages.AlertNotFound);
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpPut("{alertId}")]
        public async Task<IActionResult> updateAlertDetailsByAlertId(int alertId, [FromBody] AlertDto alertDto)
        {
            if (alertId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAlertId);
            }
            else if (alertDto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }
            else
            {
                var result = await alertService.updateAlertDetailsByAlertIdAsync(alertId, alertDto);

                if (result == null)
                {
                    return NotFound(ApplicationMessages.AlertNotFound);
                }
                else
                {
                    return Ok(result);
                }
            }
        }
    }
}