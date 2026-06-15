using Microsoft.EntityFrameworkCore;
using TeleCare.Model;
using TeleCare.Models;

namespace TeleCare.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Payer> Payers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<KPI> KPIs { get; set; }

        public DbSet<ProgramModel> Programs { get; set; }
        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<EnrollmentModel> Enrollments { get; set; }
        public DbSet<DeviceModel> Devices { get; set; }
        public DbSet<TelemetryPointModel> TelemetryPoints { get; set; }
        public DbSet<CarePlan> CarePlans { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<AdherenceRecordModel> AdherenceRecords { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<VisitNote> VisitNotes { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<User>()
                .Property(u => u.UserID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Patient" },
                new Role { RoleID = 2, RoleName = "Clinician" },
                new Role { RoleID = 3, RoleName = "Care Coordinator" },
                new Role { RoleID = 4, RoleName = "Device Technician" },
                new Role { RoleID = 5, RoleName = "Administrator" },
                new Role { RoleID = 6, RoleName = "Auditor" }
            );

            modelBuilder.Entity<PatientModel>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuditLog>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EnrollmentModel>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(e => e.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DeviceModel>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(d => d.AssignedToPatientID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TelemetryPointModel>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(t => t.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Alert>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitNote>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(v => v.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CarePlan>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(c => c.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medication>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdherenceRecordModel>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Charge>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(c => c.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Claim>()
                .HasOne<PatientModel>()
                .WithMany()
                .HasForeignKey(c => c.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.ClinicianID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VisitNote>()
                .HasOne<Appointment>()
                .WithMany()
                .HasForeignKey(v => v.AppID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VisitNote>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(v => v.ClinicianID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Alert>()
                .HasOne<Rule>()
                .WithMany()
                .HasForeignKey(a => a.RuleID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EnrollmentModel>()
                .HasOne<ProgramModel>()
                .WithMany()
                .HasForeignKey(e => e.ProgramID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EnrollmentModel>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.EnrolledBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TelemetryPointModel>()
                .HasOne<DeviceModel>()
                .WithMany()
                .HasForeignKey(t => t.DeviceID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Medication>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.PrescribedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdherenceRecordModel>()
                .HasOne<Medication>()
                .WithMany()
                .HasForeignKey(a => a.MedID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Claim>()
                .HasOne<Payer>()
                .WithMany()
                .HasForeignKey(c => c.PayerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Claim)
                .WithMany()
                .HasForeignKey(p => p.ClaimID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
