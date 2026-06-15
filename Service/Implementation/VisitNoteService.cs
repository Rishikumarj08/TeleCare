using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

namespace TeleCare.Service.Implementation
{
    public class VisitNoteService : IVisitNoteService
    {
        private readonly IVisitNoteRepository repository;
        private readonly IAuditLogService _auditLogService;

        public VisitNoteService(IVisitNoteRepository repository, IAuditLogService auditLogService)
        {
            this.repository = repository;
            _auditLogService = auditLogService;
        }

        public async Task<VisitNoteResponseDto> createVisitNoteAsync(VisitNoteCreateDto dto)
        {
            var entity = new VisitNote
            {
                AppID = dto.AppID,
                PatientID = dto.PatientID,
                ClinicianID = dto.ClinicianID,
                NoteText = dto.NoteText,
                DiagnosesJSON = dto.DiagnosesJSON,
                OrdersJSON = dto.OrdersJSON,
                AttachmentsURIJSON = dto.AttachmentsURIJSON,
                CreatedAt = DateTime.UtcNow
            };

            Validate(entity);

            var result = await repository.createVisitNoteAsync(entity);

            await _auditLogService.LogAsync(entity.PatientID, "CREATE", "VisitNote", result.NoteID,
                $"VisitNote created for Patient '{dto.PatientID}'.");

            return MapToResponseDto(result);
        }

        public async Task<List<VisitNoteResponseDto>> getAllVisitNotesAsync()
        {
            var data = await repository.getAllVisitNotesAsync();

            return data.Select(x => MapToResponseDto(x)).ToList();
        }

        public async Task<VisitNoteResponseDto?> getVisitNoteByIdAsync(int noteId)
        {
            var entity = await repository.getVisitNoteByIdAsync(noteId);

            if (entity == null) return null;

            return MapToResponseDto(entity);
        }

        public async Task<VisitNoteResponseDto?> updateVisitNoteAsync(int noteId, VisitNoteCreateDto dto)
        {
            var entity = await repository.getVisitNoteByIdAsync(noteId);

            if (entity == null) return null;

            entity.AppID = dto.AppID;
            entity.PatientID = dto.PatientID;
            entity.ClinicianID = dto.ClinicianID;
            entity.NoteText = dto.NoteText;
            entity.DiagnosesJSON = dto.DiagnosesJSON;
            entity.OrdersJSON = dto.OrdersJSON;
            entity.AttachmentsURIJSON = dto.AttachmentsURIJSON;

            Validate(entity);

            var updated = await repository.updateVisitNoteAsync(entity);

            await _auditLogService.LogAsync(entity.PatientID, "UPDATE", "VisitNote", noteId,
                $"VisitNote '{noteId}' updated.");

            return MapToResponseDto(updated);
        }

        public async Task<List<VisitNoteResponseDto>> getFilteredVisitNotesAsync(VisitNoteQueryDto query)
        {
            var data = await repository.getFilteredVisitNotesAsync(query);

            return data.Select(x => MapToResponseDto(x)).ToList();
        }

        private static VisitNoteResponseDto MapToResponseDto(VisitNote entity)
        {
            return new VisitNoteResponseDto
            {
                NoteID = entity.NoteID,
                AppID = entity.AppID,
                PatientID = entity.PatientID,
                ClinicianID = entity.ClinicianID,
                NoteText = entity.NoteText,
                DiagnosesJSON = entity.DiagnosesJSON,
                OrdersJSON = entity.OrdersJSON,
                AttachmentsURIJSON = entity.AttachmentsURIJSON,
                CreatedAt = entity.CreatedAt
            };
        }

        private void Validate(VisitNote entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new Exception(results.First().ErrorMessage);
            }
        }
    }
}