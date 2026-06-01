using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using Microsoft.AspNetCore.Authorization;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/enrollment")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            this.enrollmentService = enrollmentService;
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> createEnrollmentRecord([FromBody] EnrollmentCreateDto enrollmentCreateDto)
        {
            if (enrollmentCreateDto == null) return BadRequest(EnrollmentConstants.RequestBodyNull);

            var result = await enrollmentService.createEnrollmentRecordAsync(enrollmentCreateDto);
            return Ok(result);
        }

        [HttpGet("{enrollmentId}")]
        [Authorize(Roles = "Patient, CareCoordinator")]
        public async Task<IActionResult> getEnrollmentDetailsByEnrollID(int enrollmentId)
        {
            if (enrollmentId <= 0) return BadRequest(EnrollmentConstants.InvalidEnrollmentId);

            var result = await enrollmentService.getEnrollmentDetailsByEnrollIDAsync(enrollmentId);
            if (result == null) return NotFound(EnrollmentConstants.EnrollmentNotFound);

            return Ok(result);
        }

        [HttpPut("{enrollmentId}")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> updateEnrollmentDetailsByEnrollID(int enrollmentId, [FromBody] EnrollmentUpdateDto enrollmentUpdateDto)
        {
            if (enrollmentId <= 0) 
                return BadRequest(EnrollmentConstants.InvalidEnrollmentId);

            var result = await enrollmentService.updateEnrollmentDetailsByEnrollIDAsync(enrollmentId, enrollmentUpdateDto);
            if (result == null) return NotFound(EnrollmentConstants.EnrollmentNotFound);

            return Ok(result);
        }

        [HttpGet("filter")]
        [Authorize(Roles = "Patient, CareCoordinator")]
        public async Task<IActionResult> getFilteredEnrollmentRecords([FromQuery] EnrollmentQueryDto enrollmentQueryDto)
        {
            return Ok(await enrollmentService.getFilteredEnrollmentRecordsAsync(enrollmentQueryDto));
        }
    }
}