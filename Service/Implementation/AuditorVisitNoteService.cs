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
            // Join VisitNotes with Patients on PatientReferenceNumber = PatientID
            var result = await _context.VisitNotes
                .Join(
                    _context.Patients,
                    v => v.PatientReferenceNumber,
                    p => p.PatientID,
                    (v, p) => new AuditorVisitNoteResponseDto
                    {
                        PatientName = p.Name,
                        Notes = v.Notes,
                        Diagnosis = v.Diagnosis,
                        Orders = v.Orders,
                        AttachmentName = v.AttachmentName,
                        VisitNoteStatus = v.VisitNoteStatus.ToString(),
                        CreatedOn = v.CreatedOn
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
            // Join VisitNotes with Patients on PatientReferenceNumber = PatientID
            var query = _context.VisitNotes
                .Join(
                    _context.Patients,
                    v => v.PatientReferenceNumber,
                    p => p.PatientID,
                    (v, p) => new
                    {
                        VisitNote = v,
                        PatientName = p.Name
                    }
                )
                .AsQueryable();

            // Filter by PatientName
            if (!string.IsNullOrWhiteSpace(searchDto.PatientName))
                query = query.Where(x => x.PatientName.Contains(searchDto.PatientName));

            // Filter by Status
            if (!string.IsNullOrWhiteSpace(searchDto.VisitNoteStatus))
                query = query.Where(x => x.VisitNote.VisitNoteStatus.ToString().ToLower() == searchDto.VisitNoteStatus.Trim().ToLower());

            // Free text search in Notes and Diagnosis
            if (!string.IsNullOrWhiteSpace(searchDto.SearchText))
                query = query.Where(x =>
                    x.VisitNote.Notes.Contains(searchDto.SearchText) ||
                    x.VisitNote.Diagnosis.Contains(searchDto.SearchText));

            var result = await query
                .Select(x => new AuditorVisitNoteResponseDto
                {
                    PatientName = x.PatientName,
                    Notes = x.VisitNote.Notes,
                    Diagnosis = x.VisitNote.Diagnosis,
                    Orders = x.VisitNote.Orders,
                    AttachmentName = x.VisitNote.AttachmentName,
                    VisitNoteStatus = x.VisitNote.VisitNoteStatus.ToString(),
                    CreatedOn = x.VisitNote.CreatedOn
                })
                .OrderByDescending(v => v.CreatedOn)
                .ToListAsync();

            if (result == null || result.Count == 0)
                throw new NotFoundException(AppConstants.NoVisitNotesFound);

            return result;
        }
    }
}
