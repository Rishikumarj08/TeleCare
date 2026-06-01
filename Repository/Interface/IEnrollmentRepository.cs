using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IEnrollmentRepository
    {
        Task<EnrollmentModel> createEnrollmentRecordAsync(EnrollmentModel createEnrollment);
        Task<EnrollmentModel?> getEnrollmentRecordByEnrollIDAsync(int enrollId);
        Task<EnrollmentModel> updateEnrollmentRecordByEnrollIDAsync(EnrollmentModel updateEnrollment);
        Task<List<EnrollmentModel>> getFilteredEnrollmentRecordsAsync(EnrollmentQueryDto enrollmentQueryDto);
    }
}