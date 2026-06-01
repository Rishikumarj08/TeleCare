using TeleCare.Dto;
using TeleCare.Models;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repository;

        public MedicationService(IMedicationRepository repository)
        {
            _repository = repository;
        }

        //  CREATE
        public async Task<MedicationResponseDto?> CreateMedicationAsync(int patientId, MedicationRequestDto dto)
        {
            if (dto == null || patientId <= 0)
                return null;

            if (dto.EndAt.HasValue && dto.EndAt < dto.StartAt)
                return null;

            var entity = new Medication
            {
                PatientId = patientId,
                Name = dto.Name,
                Dose = dto.Dose,
                Frequency = dto.Frequency,
                Route = dto.Route,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Status = dto.Status
            };

            var result = await _repository.CreateMedicationAsync(entity);

            return Map(result);
        }

        //  GET BY ID
        public async Task<MedicationResponseDto?> GetMedicationByIdAsync(int id)
        {
            if (id <= 0) return null;

            var entity = await _repository.GetMedicationByIdAsync(id);

            if (entity == null) return null;

            return Map(entity);
        }

        //  UPDATE
        public async Task<MedicationResponseDto?> UpdateMedicationAsync(int id, MedicationRequestDto dto)
        {
            if (dto == null || id <= 0)
                return null;

            if (dto.EndAt.HasValue && dto.EndAt < dto.StartAt)
                return null;

            var entity = new Medication
            {
                Name = dto.Name,
                Dose = dto.Dose,
                Frequency = dto.Frequency,
                Route = dto.Route,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Status = dto.Status
            };

            var updated = await _repository.UpdateMedicationAsync(id, entity);

            if (updated == null) return null;

            return Map(updated);
        }

        //  GET ALL
        public async Task<IEnumerable<MedicationResponseDto>> GetAllMedicationsAsync(MedicationSearchDto searchDto)
        {
            var list = await _repository.GetAllMedicationsAsync(searchDto);

            return list.Select(Map);
        }

        //  MAP
        private static MedicationResponseDto Map(Medication m)
        {
            return new MedicationResponseDto
            {
                Name = m.Name,
                Dose = m.Dose,
                Frequency = m.Frequency,
                Route = m.Route,
                StartAt = m.StartAt,
                EndAt = m.EndAt,
                Status = m.Status,
                StatusLabel = m.Status.ToString()
            };
        }
    }
}