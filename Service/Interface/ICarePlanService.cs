using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface ICarePlanService
    {
        // ✅ Get All CarePlans (with search + pagination)
        Task<List<CarePlanResponseDTO>> GetAllCarePlansAsync(CarePlanSearchDTO searchDTO);

        // ✅ Get CarePlan by ID
        Task<CarePlanResponseDTO?> GetCarePlanByIdAsync(int id);

        // ✅ Create new CarePlan
        Task<CarePlanResponseDTO?> CreateCarePlanAsync(CarePlanCreateDTO dto);

        // ✅ Update existing CarePlan
        Task<CarePlanResponseDTO?> UpdateCarePlanAsync(int id, CarePlanUpdateDTO dto);
    }
}