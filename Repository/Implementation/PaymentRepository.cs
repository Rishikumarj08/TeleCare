namespace TeleCare.Repository.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Data;
    using TeleCare.DTO;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
 
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
 
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }
 
        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Claim)
                    .ThenInclude(c => c!.Patient)
                .Include(p => p.Claim)
                    .ThenInclude(c => c!.Payer)
                .ToListAsync();
        }
 
        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Claim)
                    .ThenInclude(c => c!.Patient)
                .Include(p => p.Claim)
                    .ThenInclude(c => c!.Payer)
                .FirstOrDefaultAsync(p => p.PaymentID == paymentId);
        }
 
        public async Task<List<Payment>> SearchPaymentsAsync(SearchPaymentDto searchDto)
        {
            var query = _context.Payments
                .Include(p => p.Claim)
                    .ThenInclude(c => c!.Patient)
                .Include(p => p.Claim)
                    .ThenInclude(c => c!.Payer)
                .AsQueryable();
 
            if (searchDto.PaymentID.HasValue)
                query = query.Where(p => p.PaymentID == searchDto.PaymentID.Value);
 
            if (!string.IsNullOrWhiteSpace(searchDto.PatientName))
                query = query.Where(p => p.Claim != null && p.Claim.Patient != null &&
                    p.Claim.Patient.Name.Contains(searchDto.PatientName));
 
            if (!string.IsNullOrWhiteSpace(searchDto.PayerName))
                query = query.Where(p => p.Claim != null && p.Claim.Payer != null &&
                    p.Claim.Payer.PayerName.Contains(searchDto.PayerName));
 
            if (!string.IsNullOrWhiteSpace(searchDto.Method))
                query = query.Where(p => p.Method.ToLower() == searchDto.Method.Trim().ToLower());
 
            if (!string.IsNullOrWhiteSpace(searchDto.Status))
                query = query.Where(p => p.Status.ToLower() == searchDto.Status.Trim().ToLower());
 
            return await query.ToListAsync();
        }
 
        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
 
        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
 
        public async Task DeletePaymentAsync(Payment payment)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }
}
 
 