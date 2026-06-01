using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService patientService;

        public PatientController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        [HttpPost]
        public async Task<IActionResult> createPatientRecord([FromBody] PatientCreateDto patientCreateDto)
        {
            if (patientCreateDto == null) return BadRequest(PatientConstants.RequestBodyNull);

            var result = await patientService.createPatientRecordAsync(patientCreateDto);
            return Ok(result);
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> getPatientDetailsByPatientId(int patientId)
        {
            if (patientId <= 0) return BadRequest(PatientConstants.InvalidPatientId);

            var result = await patientService.getPatientDetailsByPatientIdAsync(patientId);
            if (result == null) return NotFound(PatientConstants.PatientNotFound);

            return Ok(result);
        }

        [HttpPut("{patientId}")]
        public async Task<IActionResult> updatePatientDetailsByPatientId(int patientId, [FromBody] PatientUpdateDto patientUpdateDto)
        {
            if (patientId <= 0) 
                return BadRequest(PatientConstants.InvalidPatientId);

            var result = await patientService.updatePatientDetailsByPatientIdAsync(patientId, patientUpdateDto);
            if (result == null) return NotFound(PatientConstants.PatientNotFound);

            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> getFilteredPatientRecords([FromQuery] PatientQueryDto patientQueryDto)
        {
            return Ok(await patientService.getFilteredPatientRecordsAsync(patientQueryDto));
        }
    }
}