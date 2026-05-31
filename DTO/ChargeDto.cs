namespace TeleCare.DTO
{
    public class ChargeCreateDto
    {
        public int PatientID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public required string Status { get; set; }
    }
 
    public class ChargeResponseDto
    {
        public int ChargeID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public required string Status { get; set; }
    }
 
    public class SearchChargeDto
    {
        public int? ChargeID { get; set; }
        public string? PatientName { get; set; }
        public DateTime? Date { get; set; }
        public string? Status { get; set; }
    }
}
 
 