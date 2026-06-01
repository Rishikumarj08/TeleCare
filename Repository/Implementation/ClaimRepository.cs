namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.DTO;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
 
    public class ClaimRepository : IClaimRepository
    {
        private readonly AppDbContext _context;
 
        public ClaimRepository(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<List<Claim>> GetAllClaimsAsync()
        {
            return await _context.Claims
                .Include(c => c.Patient)
                .Include(c => c.Payer)
                .ToListAsync();
        }
 
        public async Task<Claim?> GetClaimByIdAsync(int claimId)
        {
            return await _context.Claims
                .Include(c => c.Patient)
                .Include(c => c.Payer)
                .FirstOrDefaultAsync(c => c.ClaimID == claimId);
        }
 
        public async Task<List<Claim>> SearchClaimsAsync(SearchClaimDto searchDto)
        {
            var query = _context.Claims
                .Include(c => c.Patient)
                .Include(c => c.Payer)
                .AsQueryable();
 
            if (searchDto.ClaimID.HasValue)
                query = query.Where(c => c.ClaimID == searchDto.ClaimID.Value);
 
            if (!string.IsNullOrWhiteSpace(searchDto.PatientName))
                query = query.Where(c => c.Patient != null && c.Patient.Name.Contains(searchDto.PatientName));
 
            if (!string.IsNullOrWhiteSpace(searchDto.PayerName))
                query = query.Where(c => c.Payer != null && c.Payer.PayerName.Contains(searchDto.PayerName));
 
            if (!string.IsNullOrWhiteSpace(searchDto.Status))
                query = query.Where(c => c.Status.ToLower() == searchDto.Status.Trim().ToLower());
 
            if (searchDto.SubmittedAt.HasValue)
                query = query.Where(c => c.SubmittedAt.Date == searchDto.SubmittedAt.Value.Date);
 
            return await query.ToListAsync();
        }
 
        public async Task AddClaimAsync(Claim claim)
        {
            await _context.Claims.AddAsync(claim);
            await _context.SaveChangesAsync();
        }
 
        public async Task UpdateClaimAsync(Claim claim)
        {
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();
        }
 
        public async Task DeleteClaimAsync(Claim claim)
        {
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();
        }
    }
}
 
 