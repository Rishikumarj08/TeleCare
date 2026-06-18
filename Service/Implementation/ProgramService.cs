using System.Security.Claims;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _repository;
        private readonly IAuditLogService _auditLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProgramService(IProgramRepository repository, IAuditLogService auditLogService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _auditLogService = auditLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId() =>
            int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<List<ProgramResponseDTO>> GetAllProgramsAsync(ProgramSearchDTO searchDTO)
        {
            var programs = await _repository.GetAllProgramsAsync(searchDTO);

            return programs.Select(p => new ProgramResponseDTO
            {
                ProgramID = p.ProgramID,
                ProgramName = p.ProgramName,
                Description = p.Description,
                Status = p.Status.ToString(),
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public async Task<ProgramResponseDTO?> GetProgramByIdAsync(int id)
        {
            if (id <= 0) return null;

            var program = await _repository.GetProgramByIdAsync(id);

            if (program == null) return null;

            return new ProgramResponseDTO
            {
                ProgramID = program.ProgramID,
                ProgramName = program.ProgramName,
                Description = program.Description,
                Status = program.Status.ToString(),
                CreatedAt = program.CreatedAt
            };
        }

        public async Task<ProgramResponseDTO?> CreateProgramAsync(ProgramCreateDTO dto)
        {
            var entity = new ProgramModel
            {
                ProgramName = dto.ProgramName,
                Description = dto.Description,
                Status = dto.Status
            };

            await _repository.AddProgramAsync(entity);
            await _auditLogService.LogAsync(GetCurrentUserId(), "CREATE", "Program", entity.ProgramID,
                $"Program '{entity.ProgramName}' created.");

            return new ProgramResponseDTO
            {
                ProgramID = entity.ProgramID,
                ProgramName = entity.ProgramName,
                Description = entity.Description,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<ProgramResponseDTO?> UpdateProgramAsync(ProgramUpdateDTO dto)
        {
            var entity = await _repository.GetProgramByIdAsync(dto.ProgramID);

            if (entity == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.ProgramName))
                entity.ProgramName = dto.ProgramName;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                entity.Description = dto.Description;

            entity.Status = dto.Status;

            await _repository.UpdateProgramAsync(entity);
            await _auditLogService.LogAsync(GetCurrentUserId(), "UPDATE", "Program", entity.ProgramID,
                $"Program '{entity.ProgramName}' updated. Status: '{dto.Status}'.");

            return new ProgramResponseDTO
            {
                ProgramID = entity.ProgramID,
                ProgramName = entity.ProgramName,
                Description = entity.Description,
                Status = entity.Status.ToString(),
                CreatedAt = entity.CreatedAt
            };
        }
    }
}
