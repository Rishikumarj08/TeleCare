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
        public async Task<IActionResult> createAppointment([FromBody] AppointmentCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }

            var result = await appointmentService.createAppointmentAsync(dto);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Clinician,Patient")]
        public async Task<IActionResult> getAllAppointments()
        {
            var result = await appointmentService.getAllAppointmentsAsync();
            return Ok(result);
        }

        [HttpGet("{appointmentId}")]
        [Authorize(Roles = "Clinician,Patient")]
        public async Task<IActionResult> getAppointmentById(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAppointmentId);
            }

            var result = await appointmentService.getAppointmentByIdAsync(appointmentId);

            if (result == null)
            {
                return NotFound(ApplicationMessages.AppointmentNotFound);
            }

            return Ok(result);
        }

        [HttpPut("{appointmentId}")]
        [Authorize(Roles = "Clinician")]
        public async Task<IActionResult> updateAppointment(int appointmentId, [FromBody] AppointmentCreateDto dto)
        {
            if (appointmentId <= 0)
            {
                return BadRequest(ApplicationMessages.InvalidAppointmentId);
            }

            if (dto == null)
            {
                return BadRequest(ApplicationMessages.RequestBodyNull);
            }

            var result = await appointmentService.updateAppointmentAsync(appointmentId, dto);

            if (result == null)
            {
                return NotFound(ApplicationMessages.AppointmentNotFound);
            }

            return Ok(result);
        }

        [HttpGet("filter")]
        [Authorize(Roles = "Clinician,Patient")]
        public async Task<IActionResult> getFilteredAppointments([FromQuery] AppointmentQueryDto query)
        {
            var result = await appointmentService.getFilteredAppointmentsAsync(query);
            return Ok(result);
        }
    }
}