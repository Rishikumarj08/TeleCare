using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;
using TeleCare.Enum;

namespace TeleCare.Service.Implementation
{
    public class CarePlanService : ICarePlanService
    {
        private readonly ICarePlanRepository _repository;

        public CarePlanService(ICarePlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CarePlanResponseDTO>> GetAllCarePlansAsync(CarePlanSearchDTO searchDTO)
        {
            var list = await _repository.GetAllCarePlansAsync(searchDTO);

            return list.Select(x => new CarePlanResponseDTO
            {
                PlanName = x.PlanName,
                Description = x.Description,
                Status = x.Status,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            }).ToList();
        }

        public async Task<CarePlanResponseDTO?> GetCarePlanByIdAsync(int id)
        {
            if (id <= 0) return null;

            var entity = await _repository.GetCarePlanByIdAsync(id);
            if (entity == null) return null;

            return new CarePlanResponseDTO
            {
                PlanName = entity.PlanName,
                Description = entity.Description,
                Status = entity.Status,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };
        }

        public async Task<CarePlanResponseDTO?> CreateCarePlanAsync(CarePlanCreateDTO dto)
        {
            if (dto.EndDate.HasValue && dto.EndDate < dto.StartDate)
                throw new Exception("EndDate cannot be less than StartDate");

            var entity = new CarePlan
            {
                PatientID = dto.PatientID,
                ProgramID = dto.ProgramID,
                PlanName = dto.PlanName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = CarePlanStatus.Active
            };

            await _repository.AddCarePlanAsync(entity);

            return new CarePlanResponseDTO
            {
                PlanName = entity.PlanName,
                Description = entity.Description,
                Status = entity.Status,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };
        }

        public async Task<CarePlanResponseDTO?> UpdateCarePlanAsync(int id, CarePlanUpdateDTO dto)
        {
            var entity = await _repository.GetCarePlanByIdAsync(id);

            if (entity == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.PlanName))
                entity.PlanName = dto.PlanName;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                entity.Description = dto.Description;

            entity.Status = dto.Status;

            await _repository.UpdateCarePlanAsync(entity);

            return new CarePlanResponseDTO
            {
                PlanName = entity.PlanName,
                Description = entity.Description,
                Status = entity.Status,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };
        }
    }
}