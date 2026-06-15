using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface ICarePlanService
    {
        Task<List<CarePlanResponseDTO>> GetAllCarePlansAsync(CarePlanSearchDTO searchDTO);

        Task<CarePlanResponseDTO?> GetCarePlanByIdAsync(int id);

        Task<CarePlanResponseDTO?> CreateCarePlanAsync(CarePlanCreateDTO dto);

        Task<CarePlanResponseDTO?> UpdateCarePlanAsync(int id, CarePlanUpdateDTO dto);
    }
}