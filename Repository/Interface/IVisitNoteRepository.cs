using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IVisitNoteRepository
    {
        Task<VisitNote> createVisitNoteAsync(VisitNote visitNote);

        Task<List<VisitNote>> getAllVisitNotesAsync();

        Task<VisitNote?> getVisitNoteByIdAsync(int noteId);

        Task<VisitNote> updateVisitNoteAsync(VisitNote visitNote);

        Task<List<VisitNote>> getFilteredVisitNotesAsync(VisitNoteQueryDto queryDto);
    }
}
