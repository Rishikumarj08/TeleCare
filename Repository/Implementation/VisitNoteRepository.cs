using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class VisitNoteRepository : IVisitNoteRepository
    {
        private readonly AppDbContext context;

        public VisitNoteRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<VisitNote> createVisitNoteAsync(VisitNote visitNote)
        {
            await context.VisitNotes.AddAsync(visitNote);
            await context.SaveChangesAsync();
            return visitNote;
        }

        public async Task<List<VisitNote>> getAllVisitNotesAsync()
        {
            return await context.VisitNotes.ToListAsync();
        }

        public async Task<VisitNote?> getVisitNoteByIdAsync(int noteId)
        {
            return await context.VisitNotes
                .FirstOrDefaultAsync(x => x.NoteID == noteId);
        }

        public async Task<VisitNote> updateVisitNoteAsync(VisitNote visitNote)
        {
            context.VisitNotes.Update(visitNote);
            await context.SaveChangesAsync();
            return visitNote;
        }

        public async Task<List<VisitNote>> getFilteredVisitNotesAsync(VisitNoteQueryDto queryDto)
        {
            var query = context.VisitNotes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryDto.SearchText))
            {
                query = query.Where(x =>
                    x.NoteText.Contains(queryDto.SearchText) ||
                    (x.DiagnosesJSON != null && x.DiagnosesJSON.Contains(queryDto.SearchText)));
            }

            if (queryDto.PatientID.HasValue)
            {
                query = query.Where(x => x.PatientID == queryDto.PatientID.Value);
            }

            if (queryDto.ClinicianID.HasValue)
            {
                query = query.Where(x => x.ClinicianID == queryDto.ClinicianID.Value);
            }

            return await query.ToListAsync();
        }
    }
}