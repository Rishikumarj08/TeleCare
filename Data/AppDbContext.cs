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
        
    }
}
