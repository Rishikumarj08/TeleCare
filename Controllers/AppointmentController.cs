using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using Microsoft.AspNetCore.Authorization;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this.appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> createAppointmentRecord([FromBody] AppointmentDto dto)
        {
            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }
            else
            {
                var result = await appointmentService.createAppointmentRecordAsync(dto);
                return Ok(result);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Clinician,Patient")]
        public async Task<IActionResult> getAllAppointmentRecords()
        {
            var result = await appointmentService.getAllAppointmentRecordsAsync();
            return Ok(result);
        }

        [HttpGet("{appointmentId}")]
        [Authorize(Roles = "Clinician,Patient")]
        public async Task<IActionResult> getAppointmentDetailsByAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAppointmentId);
            }
            else
            {
                var result = await appointmentService.getAppointmentDetailsByAppointmentIdAsync(appointmentId);

                if (result == null)
                {
                    return NotFound(ApplicationMessages.AppointmentNotFound);
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpPut("{appointmentId}")]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> updateAppointmentDetailsByAppointmentId(int appointmentId, [FromBody] AppointmentDto dto)
        {
            if (appointmentId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAppointmentId);
            }
            else if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }
            else
            {
                var result = await appointmentService.updateAppointmentDetailsByAppointmentIdAsync(appointmentId, dto);

                if (result == null)
                {
                    return NotFound(ApplicationMessages.AppointmentNotFound);
                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpGet("filter")]
        [Authorize(Roles = "Clinician,Patient")]
        public async Task<IActionResult> getFilteredAppointmentRecords([FromQuery] AppointmentQueryDto query)
        {
            var result = await appointmentService.getFilteredAppointmentRecordsAsync(query);
            return Ok(result);
        }
    }
}