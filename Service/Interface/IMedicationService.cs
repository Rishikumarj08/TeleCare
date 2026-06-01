using TeleCare.Dto;

namespace TeleCare.Service.Interface
{
    public interface IMedicationService
    {
        Task<MedicationResponseDto?> CreateMedicationAsync(int patientId, MedicationRequestDto dto);
        Task<MedicationResponseDto?> UpdateMedicationAsync(int medicationId, MedicationRequestDto dto);
        Task<MedicationResponseDto?> GetMedicationByIdAsync(int medicationId);

        Task<IEnumerable<MedicationResponseDto>> GetAllMedicationsAsync(MedicationSearchDto searchDto);
    }
}