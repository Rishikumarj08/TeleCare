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

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<VisitNote> VisitNotes { get; set; }
        public DbSet<Alert> Alerts { get; set; }

    }
}