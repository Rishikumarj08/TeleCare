using Microsoft.EntityFrameworkCore;
using TeleCare.Model;
using TeleCare.Models;

namespace TeleCare.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProgramModel> Programs { get; set; }
        
        public DbSet<Medication> Medications { get; set; }
        public DbSet<CarePlan> CarePlans { get; set; }
        
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<VisitNote> VisitNotes { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<EnrollmentModel> Enrollments { get; set; }
        public DbSet<TelemetryPointModel> TelemetryPoints { get; set; }
        public DbSet<AdherenceRecordModel> AdherenceRecords { get; set; }
        public DbSet<DeviceModel> Devices { get; set; }
        
    }

}
