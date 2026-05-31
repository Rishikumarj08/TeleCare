using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly AppDbContext context;

        public DeviceRepository(AppDbContext context) => this.context = context;

        public async Task<DeviceModel> createDeviceRecordAsync(DeviceModel createDevice)
        {
            await context.Devices.AddAsync(createDevice);
            await context.SaveChangesAsync();
            return createDevice;
        }

        public async Task<DeviceModel?> getDeviceRecordByDeviceIdAsync(int deviceId)
        {
            return await context.Devices.FirstOrDefaultAsync(x => x.DeviceID == deviceId);
        }

        public async Task<DeviceModel> updateDeviceRecordByDeviceIdAsync(DeviceModel updateDevice)
        {
            context.Devices.Update(updateDevice);
            await context.SaveChangesAsync();
            return updateDevice;
        }

        public async Task deleteDeviceRecordAsync(DeviceModel deleteDevice)
        {
            context.Devices.Remove(deleteDevice);
            await context.SaveChangesAsync();
        }

        public async Task<List<DeviceModel>> getFilteredDeviceRecordsAsync(DeviceQueryDto deviceQueryDto)
        {
            var query = context.Devices.AsQueryable();

            if (!string.IsNullOrWhiteSpace(deviceQueryDto.SearchText))
            {
                query = query.Where(x => 
                    x.SerialNumber.Contains(deviceQueryDto.SearchText) || 
                    x.DeviceType.Contains(deviceQueryDto.SearchText));
            }

            if (!string.IsNullOrWhiteSpace(deviceQueryDto.Model))
                query = query.Where(x => x.Model.Contains(deviceQueryDto.Model));
            if (deviceQueryDto.Status.HasValue)
                query = query.Where(x => x.Status == deviceQueryDto.Status.Value);

            return await query.ToListAsync();
        }
    }
}