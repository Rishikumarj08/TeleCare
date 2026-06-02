using System.ComponentModel.DataAnnotations;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;
using TeleCare.Enum;

namespace TeleCare.Service.Implementation
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository repository;
        private readonly IAuditLogService _auditLogService;

        public EnrollmentService(IEnrollmentRepository repository, IAuditLogService auditLogService)
        {
            this.repository = repository;
            _auditLogService = auditLogService;
        }

        public async Task<EnrollmentResponseDto> createEnrollmentRecordAsync(EnrollmentCreateDto enrollmentCreateDto)
        {
            var entity = new EnrollmentModel
            {
                PatientID = enrollmentCreateDto.PatientID,
                ProgramID = enrollmentCreateDto.ProgramID,
                EnrolledBy = enrollmentCreateDto.EnrolledBy,
                ConsentDocumentURI = enrollmentCreateDto.ConsentDocumentURI,
                EnrolledAt = DateTime.UtcNow,
                Status = EnrollmentStatus.Active
            };

            Validate(entity);

            var result = await repository.createEnrollmentRecordAsync(entity);
            await _auditLogService.LogAsync(entity.EnrolledBy, "CREATE", "Enrollment", result.EnrollID,
                $"Patient '{result.PatientID}' enrolled in program '{result.ProgramID}'.");
            return MapToDto(result);
        }

        public async Task<EnrollmentResponseDto?> getEnrollmentDetailsByEnrollIDAsync(int enrollId)
        {
            var entity = await repository.getEnrollmentRecordByEnrollIDAsync(enrollId);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<EnrollmentResponseDto?> updateEnrollmentDetailsByEnrollIDAsync(int enrollId, EnrollmentUpdateDto dto)
        {
            var entity = await repository.getEnrollmentRecordByEnrollIDAsync(enrollId);
            if (entity == null) return null;

            entity.ConsentDocumentURI = dto.ConsentDocumentURI;
            entity.Status = dto.Status;

            Validate(entity);

            var updated = await repository.updateEnrollmentRecordByEnrollIDAsync(entity);
            await _auditLogService.LogAsync(entity.EnrolledBy, "UPDATE", "Enrollment", enrollId,
                $"Enrollment '{enrollId}' updated. Status: '{dto.Status}'.");
            return MapToDto(updated);
        }

        public async Task<List<EnrollmentResponseDto>> getFilteredEnrollmentRecordsAsync(EnrollmentQueryDto enrollmentQueryDto)
        {
            var data = await repository.getFilteredEnrollmentRecordsAsync(enrollmentQueryDto);
            return data.Select(MapToDto).ToList();
        }

        private void Validate(EnrollmentModel entity)
        {
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, results, true))
            {
                throw new ArgumentException(results.First().ErrorMessage);
            }
        }

        private EnrollmentResponseDto MapToDto(EnrollmentModel enrollmentModel) => new EnrollmentResponseDto
        {
            EnrollID = enrollmentModel.EnrollID,
            PatientID = enrollmentModel.PatientID,
            ProgramID = enrollmentModel.ProgramID,
            EnrolledBy = enrollmentModel.EnrolledBy,
            EnrolledAt = enrollmentModel.EnrolledAt,
            ConsentDocumentURI = enrollmentModel.ConsentDocumentURI,
            Status = enrollmentModel.Status
        };
    }
}
