namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.DTO;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
 
    public class ChargeRepository : IChargeRepository
    {
        private readonly AppDbContext _context;
 
        public ChargeRepository(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<List<Charge>> GetAllChargesAsync()
        {
            return await _context.Charges
                .Include(c => c.Patient)
                .ToListAsync();
        }
 
        public async Task<Charge?> GetChargeByIdAsync(int chargeId)
        {
            return await _context.Charges
                .Include(c => c.Patient)
                .FirstOrDefaultAsync(c => c.ChargeID == chargeId);
        }
 
        public async Task<List<Charge>> SearchChargesAsync(SearchChargeDto searchDto)
        {
            var query = _context.Charges
                .Include(c => c.Patient)
                .AsQueryable();
 
            if (searchDto.ChargeID.HasValue)
                query = query.Where(c => c.ChargeID == searchDto.ChargeID.Value);
 
            if (!string.IsNullOrWhiteSpace(searchDto.PatientName))
                query = query.Where(c => c.Patient != null && c.Patient.Name.Contains(searchDto.PatientName));
 
            if (searchDto.Date.HasValue)
                query = query.Where(c => c.Date.Date == searchDto.Date.Value.Date);
 
            if (!string.IsNullOrWhiteSpace(searchDto.Status))
                query = query.Where(c => c.Status.ToLower() == searchDto.Status.Trim().ToLower());
 
            return await query.ToListAsync();
        }
 
        public async Task AddChargeAsync(Charge charge)
        {
            await _context.Charges.AddAsync(charge);
            await _context.SaveChangesAsync();
        }
 
        public async Task UpdateChargeAsync(Charge charge)
        {
            _context.Charges.Update(charge);
            await _context.SaveChangesAsync();
        }
 
        public async Task DeleteChargeAsync(Charge charge)
        {
            _context.Charges.Remove(charge);
            await _context.SaveChangesAsync();
        }
    }
}
 
 