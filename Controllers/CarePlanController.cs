
using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using Microsoft.AspNetCore.Authorization;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/careplans")]
    public class CarePlanController : ControllerBase
    {
        private readonly ICarePlanService _service;

        public CarePlanController(ICarePlanService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Patient,Care Coordinator")]
        public async Task<IActionResult> GetAllCarePlans([FromQuery] CarePlanSearchDTO dto)
        {
            var result = await _service.GetAllCarePlansAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Care Coordinator")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(CarePlanConstants.InvalidRequest);

            var result = await _service.GetCarePlanByIdAsync(id);

            if (result == null)
                return NotFound(CarePlanConstants.CarePlanNotFound);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Care Coordinator")]
        public async Task<IActionResult> Create([FromBody] CarePlanCreateDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
                return BadRequest(ModelState);

            var result = await _service.CreateCarePlanAsync(dto);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Care Coordinator")]

        public async Task<IActionResult> Update(int id, [FromBody] CarePlanUpdateDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
                return BadRequest(ModelState);

            var result = await _service.UpdateCarePlanAsync(id, dto);

            if (result == null)
                return NotFound(CarePlanConstants.CarePlanNotFound);

            return Ok(result);
        }
    }
}
