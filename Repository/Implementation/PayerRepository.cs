namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
 
    public class PayerRepository : IPayerRepository
    {
        private readonly AppDbContext _context;
 
        public PayerRepository(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<List<Payer>> GetAllPayersAsync()
        {
            return await _context.Payers.ToListAsync();
        }
 
        public async Task<Payer?> GetPayerByIdAsync(int payerId)
        {
            return await _context.Payers.FirstOrDefaultAsync(p => p.PayerID == payerId);
        }
    }
}
 
 