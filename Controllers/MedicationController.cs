using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.Constants;
using TeleCare.Dto;
using TeleCare.Enums;
using TeleCare.Service.Interface;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/medications")]
    public class MedicationController : ControllerBase
    {
        private readonly IMedicationService _service;

        public MedicationController(IMedicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Patient,CareCoordinator")]
        public async Task<IActionResult> GetAllMedications([FromQuery] MedicationSearchDto searchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiConstants.InvalidRequest);
            }

            var result = await _service.GetAllMedicationsAsync(searchDto ?? new MedicationSearchDto());

            return Ok(result);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Administrator,Patient,CareCoordinator")]
        public async Task<IActionResult> GetMedicationsByStatus(MedicationStatus status)
        {
            var searchDto = new MedicationSearchDto
            {
                Status = status
            };

            var result = await _service.GetAllMedicationsAsync(searchDto);

            return Ok(result);
        }

        
        [HttpGet("{medicationId}")]
        [Authorize(Roles = "Patient,CareCoordinator")]
        public async Task<IActionResult> GetMedicationById(int medicationId)
        {
            if (medicationId <= 0)
            {
                return BadRequest(ApiConstants.InvalidRequest);
            }

            var result = await _service.GetMedicationByIdAsync(medicationId);

            if (result == null)
            {
                return NotFound(ApiConstants.NotFound);
            }

            return Ok(result);
        }

        
        [HttpPost]
        [Authorize(Roles = "CareCoordinator")]
        public async Task<IActionResult> CreateMedication(
            [FromQuery] int patientId,
            [FromBody] MedicationRequestDto dto)
        {
            if (!ModelState.IsValid || dto == null || patientId <= 0)
            {
                return BadRequest(ApiConstants.InvalidRequest);
            }

            var result = await _service.CreateMedicationAsync(patientId, dto);

            if (result == null)
            {
                return BadRequest(ApiConstants.InvalidRequest);
            }

            return Ok(result);
        }

        
        [HttpPut("{medicationId}")]
        [Authorize(Roles = "CareCoordinator")]
        public async Task<IActionResult> UpdateMedication(
            int medicationId,
            [FromBody] MedicationRequestDto dto)
        {
            if (!ModelState.IsValid || dto == null || medicationId <= 0)
            {
                return BadRequest(ApiConstants.InvalidRequest);
            }

            var result = await _service.UpdateMedicationAsync(medicationId, dto);

            if (result == null)
            {
                return NotFound(ApiConstants.NotFound);
            }

            return Ok(result);
        }
    }
}
