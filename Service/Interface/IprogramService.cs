using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IProgramService
    {
        Task<List<ProgramResponseDTO>> GetAllProgramsAsync(ProgramSearchDTO searchDTO);
        Task<ProgramResponseDTO?> GetProgramByIdAsync(int programId);
        Task<ProgramResponseDTO?> CreateProgramAsync(ProgramCreateDTO dto);
        Task<ProgramResponseDTO?> UpdateProgramAsync(ProgramUpdateDTO dto);
    }
}