using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> createAlert([FromBody] AlertCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }

            var result = await alertService.createAlertAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> getAllAlerts()
        {
            var result = await alertService.getAllAlertsAsync();
            return Ok(result);
        }

        [HttpGet("{alertId}")]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> getAlertById(int alertId)
        {
            if (alertId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAlertId);
            }

            var result = await alertService.getAlertByIdAsync(alertId);

            if (result == null)
            {
                return NotFound(ApplicationMessages.AlertNotFound);
            }

            return Ok(result);
        }

        [HttpPut("{alertId}")]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> updateAlert(int alertId, [FromBody] AlertCreateDto dto)
        {
            if (alertId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAlertId);
            }

            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }

            var result = await alertService.updateAlertAsync(alertId, dto);

            if (result == null)
            {
                return NotFound(ApplicationMessages.AlertNotFound);
            }

            return Ok(result);
        }
    }
}