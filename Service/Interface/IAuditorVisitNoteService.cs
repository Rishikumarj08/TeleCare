namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;

    public interface IAuditorVisitNoteService
    {
        Task<List<AuditorVisitNoteResponseDto>> GetAllVisitNotesAsync();
        Task<List<AuditorVisitNoteResponseDto>> SearchVisitNotesAsync(SearchVisitNoteDto searchDto);
    }
}
