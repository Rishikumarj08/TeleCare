using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IVisitNoteService
    {
        Task<VisitNoteResponseDto> createVisitNoteAsync(VisitNoteCreateDto dto);

        Task<List<VisitNoteResponseDto>> getAllVisitNotesAsync();

        Task<VisitNoteResponseDto?> getVisitNoteByIdAsync(int noteId);

        Task<VisitNoteResponseDto?> updateVisitNoteAsync(int noteId, VisitNoteCreateDto dto);

        Task<List<VisitNoteResponseDto>> getFilteredVisitNotesAsync(VisitNoteQueryDto queryDto);
    }
}