using Microsoft.EntityFrameworkCore;
using TeleCare.Model;

namespace TeleCare.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<EnrollmentModel> Enrollments { get; set; }
        public DbSet<TelemetryPointModel> TelemetryPoints { get; set; }
        public DbSet<AdherenceRecordModel> AdherenceRecords { get; set; }
        public DbSet<DeviceModel> Devices { get; set; }
        
    }

}