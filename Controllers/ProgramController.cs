using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TeleCare.Service.Interface;
using TeleCare.DTO;
using TeleCare.Constants;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/program")]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [HttpPost]
        [Authorize(Roles = "Care Coordinator")]
        public async Task<IActionResult> CreateProgram([FromBody] ProgramCreateDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }

            var result = await _programService.CreateProgramAsync(dto);

            if (result == null)
            {
                return BadRequest(ProgramConstants.InvalidRequest);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Patient,Care Coordinator")]
        public async Task<IActionResult> GetAllPrograms([FromQuery] ProgramSearchDTO searchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _programService.GetAllProgramsAsync(searchDTO ?? new ProgramSearchDTO());

            return Ok(result);
        }

        [HttpGet("{programId}")]
        [Authorize(Roles = "Patient,Care Coordinator")]
        public async Task<IActionResult> GetProgramById(int programId)
        {
            if (programId <= 0)
            {
                return BadRequest(ProgramConstants.InvalidRequest);
            }

            var result = await _programService.GetProgramByIdAsync(programId);

            if (result == null)
            {
                return NotFound(ProgramConstants.ProgramNotFound);
            }

            return Ok(result);
        }

        [HttpPut("{programId}")]
        [Authorize(Roles = "Care Coordinator")]
        public async Task<IActionResult> UpdateProgram(int programId, [FromBody] ProgramUpdateDTO dto)
        {
            if (!ModelState.IsValid || dto == null || programId != dto.ProgramID)
            {
                return BadRequest(ProgramConstants.InvalidRequest);
            }

            var result = await _programService.UpdateProgramAsync(dto);

            if (result == null)
            {
                return NotFound(ProgramConstants.ProgramNotFound);
            }

            return Ok(result);
        }

        [HttpDelete("{programId}")]
        [Authorize(Roles = "Care Coordinator")]
        public async Task<IActionResult> DeleteProgram(int programId)
        {
            if (programId <= 0)
            {
                return BadRequest(ProgramConstants.InvalidRequest);
            }

            var result = await _programService.GetProgramByIdAsync(programId);

            if (result == null)
            {
                return NotFound(ProgramConstants.ProgramNotFound);
            }

            return Ok(new { message = "Program deleted successfully (implement service logic)" });
        }
    }
}
