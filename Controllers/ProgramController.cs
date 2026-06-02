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

        //  CREATE PROGRAM
        [HttpPost]
        [Authorize(Roles = "CareCoordinator")]
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

        //  GET ALL WITH SEARCH
        [HttpGet]
        [Authorize(Roles = "Patient,CareCoordinator")]
        public async Task<IActionResult> GetAllPrograms([FromQuery] ProgramSearchDTO searchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _programService.GetAllProgramsAsync(searchDTO ?? new ProgramSearchDTO());

            return Ok(result);
        }

        // GET BY ID
        [HttpGet("{programId}")]
        [Authorize(Roles = "Patient,CareCoordinator")]
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

        // UPDATE PROGRAM
        [HttpPut("{programId}")]
        [Authorize(Roles = "CareCoordinator")]
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

        // DELETE PROGRAM (Optional but recommended)
        [HttpDelete("{programId}")]
        [Authorize(Roles = "CareCoordinator")]
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

            // You can implement delete in service if needed
            return Ok(new { message = "Program deleted successfully (implement service logic)" });
        }
    }
}