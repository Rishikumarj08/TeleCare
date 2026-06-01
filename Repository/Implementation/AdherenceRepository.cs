using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class AdherenceRepository : IAdherenceRepository
    {
        private readonly AppDbContext context;

        public AdherenceRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<AdherenceRecordModel> createAdherenceRecordAsync(AdherenceRecordModel adherenceRecordModel)
        {
            await context.AdherenceRecords.AddAsync(adherenceRecordModel);
            await context.SaveChangesAsync();
            return adherenceRecordModel;
        }

        public async Task<AdherenceRecordModel?> getAdherenceRecordByAdhIDAsync(int adhId)
        {
            return await context.AdherenceRecords.FirstOrDefaultAsync(x => x.AdhID == adhId);
        }

        public async Task<AdherenceRecordModel> updateAdherenceRecordByAdhIDAsync(AdherenceRecordModel adherenceRecordModel)
        {
            context.AdherenceRecords.Update(adherenceRecordModel);
            await context.SaveChangesAsync();
            return adherenceRecordModel;
        }

        public async Task<List<AdherenceRecordModel>> getFilteredAdherenceRecordsAsync(AdherenceQueryDto adherenceQueryDto)
        {
            var query = context.AdherenceRecords.AsQueryable();

            if (!string.IsNullOrWhiteSpace(adherenceQueryDto.SearchText))
                query = query.Where(x => x.Notes.Contains(adherenceQueryDto.SearchText) || x.Source.Contains(adherenceQueryDto.SearchText));

            if (adherenceQueryDto.PatientID.HasValue)
                query = query.Where(x => x.PatientID == adherenceQueryDto.PatientID.Value);

            if (adherenceQueryDto.MedID.HasValue)
                query = query.Where(x => x.MedID == adherenceQueryDto.MedID.Value);

            if (adherenceQueryDto.Status.HasValue)
                query = query.Where(x => x.Status == adherenceQueryDto.Status.Value);

            return await query.ToListAsync();
        }
    }
}