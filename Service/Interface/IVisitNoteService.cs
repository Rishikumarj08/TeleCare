using TeleCare.DTO;

namespace TeleCare.Service.Interface
{
    public interface IVisitNoteService
    {
        Task<VisitNoteDto> createVisitNoteRecordAsync(VisitNoteDto visitNoteDto);

        Task<List<VisitNoteDto>> getAllVisitNoteRecordsAsync();

        Task<VisitNoteDto> getVisitNoteDetailsByVisitNoteIdAsync(int visitNoteId);

        Task<VisitNoteDto> updateVisitNoteDetailsByVisitNoteIdAsync(int visitNoteId, VisitNoteDto visitNoteDto);

        Task<List<VisitNoteDto>> getFilteredVisitNoteRecordsAsync(VisitNoteQueryDto queryDto);

    }
}