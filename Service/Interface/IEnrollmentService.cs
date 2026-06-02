using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> createEnrollmentRecordAsync(EnrollmentCreateDto enrollmentCreateDto);
        Task<EnrollmentResponseDto?> getEnrollmentDetailsByEnrollIDAsync(int enrollId);
        Task<EnrollmentResponseDto?> updateEnrollmentDetailsByEnrollIDAsync(int enrollId, EnrollmentUpdateDto enrollmentUpdateDto);
        Task<List<EnrollmentResponseDto>> getFilteredEnrollmentRecordsAsync(EnrollmentQueryDto enrollmentQueryDto);
    }
}