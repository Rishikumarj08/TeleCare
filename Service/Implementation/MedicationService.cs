using System.Security.Claims;
using TeleCare.Dto;
using TeleCare.Models;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationRepository _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MedicationService(IMedicationRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<MedicationResponseDto?> CreateMedicationAsync(int patientId, MedicationRequestDto dto)
        {
            if (dto == null || patientId <= 0)
                return null;

            if (dto.EndAt.HasValue && dto.EndAt < dto.StartAt)
                return null;

            var prescribedBy = GetCurrentUserId();
            if (prescribedBy <= 0)
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
                Status = dto.Status,
                PrescribedBy = prescribedBy
            };

            var result = await _repository.CreateMedicationAsync(entity);
            await _auditLogService.LogAsync(patientId, "CREATE", "Medication", result.MedicationId,
                $"Medication '{result.Name}' created for patient '{patientId}'.");

            return Map(result);
        }

        public async Task<MedicationResponseDto?> GetMedicationByIdAsync(int id)
        {
            if (id <= 0) return null;

            var entity = await _repository.GetMedicationByIdAsync(id);

            if (entity == null) return null;

            return Map(entity);
        }

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

            await _auditLogService.LogAsync(GetCurrentUserId(), "UPDATE", "Medication", id,
                $"Medication '{id}' updated.");

            return Map(updated);
        }

        public async Task<IEnumerable<MedicationResponseDto>> GetAllMedicationsAsync(MedicationSearchDto searchDto)
        {
            var list = await _repository.GetAllMedicationsAsync(searchDto);

            return list.Select(Map);
        }

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
                PrescribedBy = m.PrescribedBy,
                Status = m.Status,
                StatusLabel = m.Status.ToString()
            };
        }
    }
}
