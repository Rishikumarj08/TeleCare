using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IDeviceRepository
    {
        Task<DeviceModel> createDeviceRecordAsync(DeviceModel createDevice);
        Task<DeviceModel?> getDeviceRecordByDeviceIdAsync(int deviceId);
        Task<DeviceModel> updateDeviceRecordByDeviceIdAsync(DeviceModel updateDevice);
        Task deleteDeviceRecordAsync(DeviceModel device);
        Task<List<DeviceModel>> getFilteredDeviceRecordsAsync(DeviceQueryDto deviceQueryDto);
    }
}