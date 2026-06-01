namespace TeleCare.Service.Implementation
{
    using TeleCare.Constants;
    using TeleCare.DTO;
    using TeleCare.Exceptions;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
    using TeleCare.Service.Interface;
 
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IClaimRepository _claimRepository;
 
        public PaymentService(IPaymentRepository paymentRepository, IClaimRepository claimRepository)
        {
            _paymentRepository = paymentRepository;
            _claimRepository = claimRepository;
        }
 
        public async Task<List<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            return payments.Select(Map).ToList();
        }
 
        public async Task<PaymentResponseDto> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new NotFoundException(AppConstants.PaymentNotFound);
            return Map(payment);
        }
 
        public async Task<List<PaymentResponseDto>> SearchPaymentsAsync(SearchPaymentDto searchDto)
        {
            var payments = await _paymentRepository.SearchPaymentsAsync(searchDto);
            if (payments == null || payments.Count == 0)
                throw new NotFoundException(AppConstants.NoPaymentsFound);
            return payments.Select(Map).ToList();
        }
 
        public async Task CreatePaymentAsync(PaymentCreateDto paymentDto)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(paymentDto.ClaimID);
            if (claim == null)
                throw new NotFoundException(AppConstants.ClaimNotFoundForPayment);
 
            var payment = new Payment
            {
                ClaimID = paymentDto.ClaimID,
                Amount = paymentDto.Amount,
                Method = paymentDto.Method,
                DatePaid = paymentDto.DatePaid,
                Status = paymentDto.Status
            };
 
            await _paymentRepository.AddPaymentAsync(payment);
        }
 
        public async Task UpdatePaymentAsync(int paymentId, PaymentCreateDto paymentDto)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new NotFoundException(AppConstants.PaymentNotFound);
 
            var claim = await _claimRepository.GetClaimByIdAsync(paymentDto.ClaimID);
            if (claim == null)
                throw new NotFoundException(AppConstants.ClaimNotFoundForPayment);
 
            payment.ClaimID = paymentDto.ClaimID;
            payment.Amount = paymentDto.Amount;
            payment.Method = paymentDto.Method;
            payment.DatePaid = paymentDto.DatePaid;
            payment.Status = paymentDto.Status;
 
            await _paymentRepository.UpdatePaymentAsync(payment);
        }
 
        public async Task DeletePaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new NotFoundException(AppConstants.PaymentNotFound);
            await _paymentRepository.DeletePaymentAsync(payment);
        }
 
        private static PaymentResponseDto Map(Payment payment) => new()
        {
            PaymentID = payment.PaymentID,
            ClaimID = payment.ClaimID,
            PatientName = payment.Claim?.Patient?.Name ?? string.Empty,
            PayerName = payment.Claim?.Payer?.PayerName ?? string.Empty,
            Amount = payment.Amount,
            Method = payment.Method,
            DatePaid = payment.DatePaid,
            Status = payment.Status
        };
    }
}
 
 