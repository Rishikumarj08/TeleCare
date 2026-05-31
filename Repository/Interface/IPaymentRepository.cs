namespace TeleCare.Repository.Interface
{
    using TeleCare.DTO;
    using TeleCare.Model;
 
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
        Task<List<Payment>> SearchPaymentsAsync(SearchPaymentDto searchDto);
        Task AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(Payment payment);
    }
}
 
 