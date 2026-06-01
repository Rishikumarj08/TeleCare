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

        //  GET ALL (SEARCH + FILTER)
        [HttpGet]
        //[Authorize(Roles = "Admin,Patient,CareCoordinator,Clinician")]
        public async Task<IActionResult> GetAllMedications([FromQuery] MedicationSearchDto searchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiConstants.InvalidRequest);
            }

            var result = await _service.GetAllMedicationsAsync(searchDto ?? new MedicationSearchDto());

            return Ok(result);
        }

        //  GET BY STATUS
        [HttpGet("status/{status}")]
       // [Authorize(Roles = "Admin,Patient,CareCoordinator,Clinician")]
        public async Task<IActionResult> GetMedicationsByStatus(MedicationStatus status)
        {
            var searchDto = new MedicationSearchDto
            {
                Status = status
            };

            var result = await _service.GetAllMedicationsAsync(searchDto);

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{medicationId}")]
        //[Authorize(Roles = "Admin,Patient,CareCoordinator,Clinician")]
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

        // CREATE
        [HttpPost]
       // [Authorize(Roles = "Admin,CareCoordinator")]
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

        //  UPDATE
        [HttpPut("{medicationId}")]
        //[Authorize(Roles = "Admin,CareCoordinator")]
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