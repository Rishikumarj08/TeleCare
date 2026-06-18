using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeviceService(IDeviceRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<DeviceResponseDto> createDeviceRecordAsync(DeviceCreateDto deviceCreateDto)
        {
            var entity = new DeviceModel
            {
                SerialNumber = deviceCreateDto.SerialNumber,
                Model = deviceCreateDto.Model,
                DeviceType = deviceCreateDto.DeviceType,
                AssignedToPatientID = deviceCreateDto.AssignedToPatientID,
                ProvisionedAt = DateTime.Now,
                Status = Enum.DeviceStatus.Available
            };

            Validate(entity);
            var result = await repository.createDeviceRecordAsync(entity);
            await _auditLogService.LogAsync(GetCurrentUserId(), "CREATE", "Device", result.DeviceID,
                $"Device '{result.SerialNumber}' of type '{result.DeviceType}' created.");
            return MapToDto(result);
        }

        public async Task<DeviceResponseDto?> getDeviceDetailsByDeviceIdAsync(int deviceId)
        {
            var entity = await repository.getDeviceRecordByDeviceIdAsync(deviceId);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<DeviceResponseDto?> updateDeviceDetailsByDeviceIdAsync(int deviceId, DeviceUpdateDto deviceUpdateDto)
        {
            var entity = await repository.getDeviceRecordByDeviceIdAsync(deviceId);
            if (entity == null) return null;

            entity.Model = deviceUpdateDto.Model;
            entity.Status = deviceUpdateDto.Status;
            entity.AssignedToPatientID = deviceUpdateDto.AssignedToPatientID;

            Validate(entity);
            var updated = await repository.updateDeviceRecordByDeviceIdAsync(entity);
            await _auditLogService.LogAsync(GetCurrentUserId(), "UPDATE", "Device", deviceId,
                $"Device '{deviceId}' updated. Status: '{deviceUpdateDto.Status}'.");
            return MapToDto(updated);
        }

        public async Task deleteDeviceRecordAsync(int deviceId)
        {
            var entity = await repository.getDeviceRecordByDeviceIdAsync(deviceId);
            if (entity != null)
            {
                await repository.deleteDeviceRecordAsync(entity);
                await _auditLogService.LogAsync(GetCurrentUserId(), "DELETE", "Device", deviceId,
                    $"Device '{entity.SerialNumber}' deleted.");
            }
        }

        public async Task<List<DeviceResponseDto>> getFilteredDeviceRecordsAsync(DeviceQueryDto deviceQueryDto)
        {
            var data = await repository.getFilteredDeviceRecordsAsync(deviceQueryDto);
            return data.Select(MapToDto).ToList();
        }

        private void Validate(DeviceModel entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new ArgumentException(results.First().ErrorMessage);
            }
        }

        private DeviceResponseDto MapToDto(DeviceModel deviceResponseDto) => new DeviceResponseDto
        {
            DeviceID = deviceResponseDto.DeviceID,
            SerialNumber = deviceResponseDto.SerialNumber,
            Model = deviceResponseDto.Model,
            DeviceType = deviceResponseDto.DeviceType,
            AssignedToPatientID = deviceResponseDto.AssignedToPatientID,
            ProvisionedAt = deviceResponseDto.ProvisionedAt,
            Status = deviceResponseDto.Status
        };
    }
}
