namespace TeleCare.DTO
{
    public class PaymentCreateDto
    {
        public int ClaimID { get; set; }
        public decimal Amount { get; set; }
        public required string Method { get; set; }
        public DateTime DatePaid { get; set; }
        public required string Status { get; set; }
    }
 
    public class PaymentResponseDto
    {
        public int PaymentID { get; set; }
        public int ClaimID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PayerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public required string Method { get; set; }
        public DateTime DatePaid { get; set; }
        public required string Status { get; set; }
    }
 
    public class SearchPaymentDto
    {
        public int? PaymentID { get; set; }
        public string? PatientName { get; set; }
        public string? PayerName { get; set; }
        public string? Method { get; set; }
        public string? Status { get; set; }
    }
}
 
 