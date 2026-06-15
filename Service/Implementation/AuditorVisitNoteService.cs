namespace TeleCare.Service.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Constants;
    using TeleCare.Data;
    using TeleCare.DTO;
    using TeleCare.Exceptions;
    using TeleCare.Service.Interface;

    public class AuditorVisitNoteService : IAuditorVisitNoteService
    {
        private readonly AppDbContext _context;

        public AuditorVisitNoteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditorVisitNoteResponseDto>> GetAllVisitNotesAsync()
        {
            var result = await _context.VisitNotes
                .Join(
                    _context.Patients,
                    v => v.PatientID,
                    p => p.PatientID,
                    (v, p) => new AuditorVisitNoteResponseDto
                    {
                        PatientName = p.Name,
                        Notes = v.NoteText ?? string.Empty,
                        Diagnosis = v.DiagnosesJSON ?? string.Empty,
                        Orders = v.OrdersJSON ?? string.Empty,
                        AttachmentName = v.AttachmentsURIJSON ?? string.Empty,
                        VisitNoteStatus = string.Empty,
                        CreatedOn = v.CreatedAt
                    }
                )
                .OrderByDescending(v => v.CreatedOn)
                .ToListAsync();

            if (result == null || result.Count == 0)
                throw new NotFoundException(AppConstants.NoVisitNotesFound);

            return result;
        }

        public async Task<List<AuditorVisitNoteResponseDto>> SearchVisitNotesAsync(SearchVisitNoteDto searchDto)
        {
            var query = _context.VisitNotes
                .Join(
                    _context.Patients,
                    v => v.PatientID,
                    p => p.PatientID,
                    (v, p) => new
                    {
                        VisitNote = v,
                        PatientName = p.Name
                    }
                )
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.PatientName))
                query = query.Where(x => x.PatientName.Contains(searchDto.PatientName));

            // VisitNoteStatus is not present on the VisitNote model; skip status filtering.

            if (!string.IsNullOrWhiteSpace(searchDto.SearchText))
                query = query.Where(x =>
                    (x.VisitNote.NoteText ?? string.Empty).Contains(searchDto.SearchText) ||
                    (x.VisitNote.DiagnosesJSON ?? string.Empty).Contains(searchDto.SearchText));

            var result = await query
                .Select(x => new AuditorVisitNoteResponseDto
                {
                    PatientName = x.PatientName,
                    Notes = x.VisitNote.NoteText ?? string.Empty,
                    Diagnosis = x.VisitNote.DiagnosesJSON ?? string.Empty,
                    Orders = x.VisitNote.OrdersJSON ?? string.Empty,
                    AttachmentName = x.VisitNote.AttachmentsURIJSON ?? string.Empty,
                    VisitNoteStatus = string.Empty,
                    CreatedOn = x.VisitNote.CreatedAt
                })
                .OrderByDescending(v => v.CreatedOn)
                .ToListAsync();

            if (result == null || result.Count == 0)
                throw new NotFoundException(AppConstants.NoVisitNotesFound);

            return result;
        }
    }
}
