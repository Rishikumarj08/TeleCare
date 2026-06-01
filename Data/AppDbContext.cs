using Microsoft.EntityFrameworkCore;
using TeleCare.Model;
<<<<<<< HEAD
using TeleCare.Models;
=======
 
>>>>>>> 1c322d2759f0ac9764e2db63dbdaa7c2553105a2

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
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);
           
            modelBuilder.Entity<User>()
                .Property(u => u.UserID)
                .ValueGeneratedOnAdd();
 
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Patient" },
                new Role { RoleID = 2, RoleName = "Clinician" },
                new Role { RoleID = 3, RoleName = "Care Coordinator" },
                new Role { RoleID = 4, RoleName = "Device Technician" },
                new Role { RoleID = 5, RoleName = "Administrator" },
                new Role { RoleID = 6, RoleName = "Auditor" }
            );
 
            // Claim → User (Patient)
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Patient)
                .WithMany()
                .HasForeignKey(c => c.PatientID)
                .OnDelete(DeleteBehavior.Restrict);
 
            // Claim → Payer
            modelBuilder.Entity<Claim>()
                .HasOne(c => c.Payer)
                .WithMany()
                .HasForeignKey(c => c.PayerID)
                .OnDelete(DeleteBehavior.Restrict);
 
            // Payment → Claim
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Claim)
                .WithMany()
                .HasForeignKey(p => p.ClaimID)
                .OnDelete(DeleteBehavior.Restrict);
 
            // Charge → User (Patient)
            modelBuilder.Entity<Charge>()
                .HasOne(ch => ch.Patient)
                .WithMany()
                .HasForeignKey(ch => ch.PatientID)
                .OnDelete(DeleteBehavior.Restrict);
 
            // Notification → User
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade);
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
 
 


<<<<<<< HEAD
}
=======
>>>>>>> 1c322d2759f0ac9764e2db63dbdaa7c2553105a2
