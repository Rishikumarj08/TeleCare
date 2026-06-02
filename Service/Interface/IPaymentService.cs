namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;
 
    public interface IPaymentService
    {
        Task<List<PaymentResponseDto>> GetAllPaymentsAsync();
        Task<PaymentResponseDto> GetPaymentByIdAsync(int paymentId);
        Task<List<PaymentResponseDto>> SearchPaymentsAsync(SearchPaymentDto searchDto);
        Task CreatePaymentAsync(PaymentCreateDto paymentDto);
        Task UpdatePaymentAsync(int paymentId, PaymentCreateDto paymentDto);
        Task DeletePaymentAsync(int paymentId);
    }
}
 
 