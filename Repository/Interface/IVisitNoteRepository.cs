using TeleCare.DTO;
using TeleCare.Model;

namespace TeleCare.Repository.Interface
{
    public interface IVisitNoteRepository
    {
        Task<VisitNote> createVisitNoteRecordAsync(VisitNote visitNote);

        Task<List<VisitNote>> getAllVisitNoteRecordsAsync();

        Task<VisitNote> getVisitNoteRecordByVisitNoteIdAsync(int visitNoteId);

        Task<VisitNote> updateVisitNoteRecordByVisitNoteIdAsync(VisitNote visitNote);

        Task<List<VisitNote>> getFilteredVisitNoteRecordsAsync(VisitNoteQueryDto queryDto);

    }
}