using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

namespace TeleCare.Repository.Implementation
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext context;

        public EnrollmentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<EnrollmentModel> createEnrollmentRecordAsync(EnrollmentModel createEnrollment)
        {
            await context.Enrollments.AddAsync(createEnrollment);
            await context.SaveChangesAsync();
            return createEnrollment;
        }

        public async Task<EnrollmentModel?> getEnrollmentRecordByEnrollIDAsync(int enrollId)
        {
            return await context.Enrollments.FirstOrDefaultAsync(x => x.EnrollID == enrollId);
        }

        public async Task<EnrollmentModel> updateEnrollmentRecordByEnrollIDAsync(EnrollmentModel updateEnrollment)
        {
            context.Enrollments.Update(updateEnrollment);
            await context.SaveChangesAsync();
            return updateEnrollment;
        }

        public async Task<List<EnrollmentModel>> getFilteredEnrollmentRecordsAsync(EnrollmentQueryDto enrollmentQueryDto)
        {
            var query = context.Enrollments.AsQueryable();

            if (enrollmentQueryDto.PatientID.HasValue)
                query = query.Where(x => x.PatientID == enrollmentQueryDto.PatientID.Value);

            if (enrollmentQueryDto.ProgramID.HasValue)
                query = query.Where(x => x.ProgramID == enrollmentQueryDto.ProgramID.Value);

            if (enrollmentQueryDto.Status.HasValue)
                query = query.Where(x => x.Status == enrollmentQueryDto.Status.Value);

            return await query.ToListAsync();
        }
    }
}