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

        public async Task<VisitNote?> createVisitNoteRecordAsync(VisitNote visitNote)
        {
            await context.VisitNotes.AddAsync(visitNote);
            await context.SaveChangesAsync();
            return visitNote;
        }

        public async Task<List<VisitNote>> getAllVisitNoteRecordsAsync()
        {
            return await context.VisitNotes.ToListAsync();
        }

        public async Task<VisitNote?> getVisitNoteRecordByVisitNoteIdAsync(int visitNoteId)
        {
            return await context.VisitNotes
                .FirstOrDefaultAsync(x => x.Id == visitNoteId);
        }

        public async Task<VisitNote?> updateVisitNoteRecordByVisitNoteIdAsync(VisitNote visitNote)
        {
            context.VisitNotes.Update(visitNote);
            await context.SaveChangesAsync();
            return visitNote;
        }

        public async Task<List<VisitNote>> getFilteredVisitNoteRecordsAsync(VisitNoteQueryDto queryDto)
        {
            var query = context.VisitNotes.AsQueryable();

            // CONDITION 1: Search in Notes or Diagnosis
            if (!string.IsNullOrWhiteSpace(queryDto.SearchText))
            {
                query = query.Where(x =>
                    x.Notes.Contains(queryDto.SearchText) ||
                    x.Diagnosis.Contains(queryDto.SearchText));
            }

            // CONDITION 2: Filter by PatientReferenceNumber
            if (queryDto.PatientReferenceNumber.HasValue)
            {
                query = query.Where(x => x.PatientReferenceNumber == queryDto.PatientReferenceNumber.Value);
            }

            // CONDITION 3: Filter by Status
            if (queryDto.VisitNoteStatus.HasValue)
            {
                query = query.Where(x => x.VisitNoteStatus == queryDto.VisitNoteStatus.Value);
            }

            return await query.ToListAsync();
        }
    }
}