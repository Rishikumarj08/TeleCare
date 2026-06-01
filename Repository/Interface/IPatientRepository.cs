using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IPatientRepository
    {
        Task<PatientModel> createPatientRecordAsync(PatientModel createPatientDto);
        Task<PatientModel?> getPatientRecordByPatientIdAsync(int patientId);
        Task<PatientModel> updatePatientRecordByPatientIdAsync(PatientModel updatePatientDto);
        Task<List<PatientModel>> getFilteredPatientRecordsAsync(PatientQueryDto patientQueryDto);
    }
}