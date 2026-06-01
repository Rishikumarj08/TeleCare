using TeleCare.Models;
using TeleCare.Dto;

namespace TeleCare.Repository.Interface
{
    public interface IMedicationRepository
    {
        Task<Medication> CreateMedicationAsync(Medication medication);
        Task<Medication?> GetMedicationByIdAsync(int medicationId);
        Task<Medication?> UpdateMedicationAsync(int medicationId, Medication medication);

        //  Updated for search
        Task<List<Medication>> GetAllMedicationsAsync(MedicationSearchDto searchDto);
    }
}