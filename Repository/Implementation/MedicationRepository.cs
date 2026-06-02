using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.Models;
using TeleCare.Dto;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly AppDbContext _context;

        public MedicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Medication> CreateMedicationAsync(Medication medication)
        {
            await _context.Medications.AddAsync(medication);
            await _context.SaveChangesAsync();
            return medication;
        }

        public async Task<Medication?> GetMedicationByIdAsync(int medicationId)
        {
            if (medicationId <= 0)
                return null;

            return await _context.Medications
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MedicationId == medicationId);
        }

        public async Task<Medication?> UpdateMedicationAsync(int medicationId, Medication medication)
        {
            var existing = await _context.Medications.FindAsync(medicationId);

            if (existing == null)
                return null;

            existing.Name = medication.Name;
            existing.Dose = medication.Dose;
            existing.Frequency = medication.Frequency;
            existing.Route = medication.Route;
            existing.StartAt = medication.StartAt;
            existing.EndAt = medication.EndAt;
            existing.Status = medication.Status;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<List<Medication>> GetAllMedicationsAsync(MedicationSearchDto searchDto)
        {
            var query = _context.Medications
                                .AsNoTracking()
                                .AsQueryable();

            if (searchDto != null)
            {
                if (!string.IsNullOrWhiteSpace(searchDto.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(searchDto.Keyword));
                }

                if (searchDto.Status.HasValue)
                {
                    query = query.Where(x => x.Status == searchDto.Status.Value);
                }

                if (searchDto.PatientId.HasValue)
                {
                    query = query.Where(x => x.PatientId == searchDto.PatientId.Value);
                }

                if (searchDto.StartDateFrom.HasValue)
                {
                    query = query.Where(x => x.StartAt >= searchDto.StartDateFrom.Value);
                }

                if (searchDto.StartDateTo.HasValue)
                {
                    query = query.Where(x => x.StartAt <= searchDto.StartDateTo.Value);
                }

                int pageNumber = searchDto.PageNumber <= 0 ? 1 : searchDto.PageNumber;
                int pageSize = searchDto.PageSize <= 0 ? 10 : searchDto.PageSize;

                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            return await query.ToListAsync();
        }
    }
}