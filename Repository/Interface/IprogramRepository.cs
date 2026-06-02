using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IProgramRepository
    {
        Task<List<ProgramModel>> GetAllProgramsAsync(ProgramSearchDTO searchDTO);
        Task<ProgramModel?> GetProgramByIdAsync(int programId);
        Task AddProgramAsync(ProgramModel programModel);
        Task UpdateProgramAsync(ProgramModel programModel);
    }
}