using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext context;

        public PatientRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<PatientModel> createPatientRecordAsync(PatientModel createPatientDto)
        {
            await context.Patients.AddAsync(createPatientDto);
            await context.SaveChangesAsync();
            return createPatientDto;
        }

        public async Task<PatientModel?> getPatientRecordByPatientIdAsync(int patientId)
        {
            return await context.Patients.FirstOrDefaultAsync(x => x.PatientID == patientId);
        }

        public async Task<PatientModel> updatePatientRecordByPatientIdAsync(PatientModel updatePatientDto)
        {
            context.Patients.Update(updatePatientDto);
            await context.SaveChangesAsync();
            return updatePatientDto;
        }

        public async Task<List<PatientModel>> getFilteredPatientRecordsAsync(PatientQueryDto patientQueryDto)
        {
            var query = context.Patients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(patientQueryDto.SearchText))
                query = query.Where(x => x.Name.Contains(patientQueryDto.SearchText) || x.MRN.Contains(patientQueryDto.SearchText));

            if (patientQueryDto.Status.HasValue)
                query = query.Where(x => x.Status == patientQueryDto.Status.Value);

            if (patientQueryDto.UserID.HasValue)
                query = query.Where(x => x.UserID == patientQueryDto.UserID.Value);

            return await query.ToListAsync();
        }
    }
}