using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IDeviceService
    {
        Task<DeviceResponseDto> createDeviceRecordAsync(DeviceCreateDto deviceCreateDto);
        Task<DeviceResponseDto?> getDeviceDetailsByDeviceIdAsync(int deviceId);
        Task<DeviceResponseDto?> updateDeviceDetailsByDeviceIdAsync(int deviceId, DeviceUpdateDto deviceUpdateDtodto);
        Task deleteDeviceRecordAsync(int deviceId);
        Task<List<DeviceResponseDto>> getFilteredDeviceRecordsAsync(DeviceQueryDto deviceQueryDto);
    }
}