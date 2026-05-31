namespace TeleCare.DTO
{
    public class ClaimCreateDto
    {
        public int PatientID { get; set; }
        public int PayerID { get; set; }
        public DateTime SubmittedAt { get; set; }
        public decimal AmountBilled { get; set; }
        public decimal AmountPaid { get; set; }
        public required string Status { get; set; }
    }
 
    public class ClaimResponseDto
    {
        public int ClaimID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PayerID { get; set; }
        public string PayerName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public decimal AmountBilled { get; set; }
        public decimal AmountPaid { get; set; }
        public required string Status { get; set; }
    }
 
    public class SearchClaimDto
    {
        public int? ClaimID { get; set; }
        public string? PatientName { get; set; }
        public string? PayerName { get; set; }
        public string? Status { get; set; }
        public DateTime? SubmittedAt { get; set; }
    }
}
 
 