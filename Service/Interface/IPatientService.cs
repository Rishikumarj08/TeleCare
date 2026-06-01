using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IPatientService
    {
        Task<PatientResponseDto> createPatientRecordAsync(PatientCreateDto patientCreateDto);
        Task<PatientResponseDto?> getPatientDetailsByPatientIdAsync(int patientId);
        Task<PatientResponseDto?> updatePatientDetailsByPatientIdAsync(int patientId, PatientUpdateDto patientUpdateDto);
        Task<List<PatientResponseDto>> getFilteredPatientRecordsAsync(PatientQueryDto patientQueryDto);
    }
}