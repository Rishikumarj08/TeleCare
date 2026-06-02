using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;
using TeleCare.Constants;
using Microsoft.AspNetCore.Authorization;

namespace TeleCare.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService deviceService;

        public DeviceController(IDeviceService deviceService) => this.deviceService = deviceService;

        [HttpPost]
        [Authorize(Roles = "Device Technician")]
        public async Task<IActionResult> createDeviceRecord([FromBody] DeviceCreateDto deviceCreateDto)
        {
            if (deviceCreateDto == null) return BadRequest(DeviceConstants.RequestBodyNull);
            
            var result = await deviceService.createDeviceRecordAsync(deviceCreateDto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Device Technician")]
        public async Task<IActionResult> getDeviceDetailsByDeviceId(int deviceId)
        {
            if (deviceId <= 0) return BadRequest(DeviceConstants.InvalidDeviceId);
            
            var result = await deviceService.getDeviceDetailsByDeviceIdAsync(deviceId);
            if (result == null) return NotFound(DeviceConstants.DeviceNotFound);
            
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Device Technician")]
        public async Task<IActionResult> updateDeviceDetailsByDeviceId(int deviceId, [FromBody] DeviceUpdateDto deviceUpdateDto)
        {
            if (deviceId <= 0) 
                return BadRequest(DeviceConstants.InvalidDeviceId);

            var result = await deviceService.updateDeviceDetailsByDeviceIdAsync(deviceId, deviceUpdateDto);
            if (result == null) return NotFound(DeviceConstants.DeviceNotFound);
            
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Device Technician")]
        public async Task<IActionResult> deleteDeviceRecord(int deviceId)
        {
            if (deviceId <= 0) return BadRequest(DeviceConstants.InvalidDeviceId);
            
            await deviceService.deleteDeviceRecordAsync(deviceId);
            return Ok(DeviceConstants.DeviceDeleted);
        }

        [HttpGet("filter")]
        [Authorize(Roles = "Device Technician")]
        public async Task<IActionResult> getFilteredDeviceRecords([FromQuery] DeviceQueryDto deviceQueryDto)
        {
            return Ok(await deviceService.getFilteredDeviceRecordsAsync(deviceQueryDto));
        }
    }
}
