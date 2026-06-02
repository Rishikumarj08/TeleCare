using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface ICarePlanRepository
    {
        Task<List<CarePlan>> GetAllCarePlansAsync(CarePlanSearchDTO searchDTO);
        Task<CarePlan?> GetCarePlanByIdAsync(int id);
        Task AddCarePlanAsync(CarePlan carePlan);
        Task UpdateCarePlanAsync(CarePlan carePlan);
    }
}
