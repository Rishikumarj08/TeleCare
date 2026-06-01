namespace TeleCare.DTO
{
    /// <summary>
    /// Used only as a lookup response when displaying PayerName in Claims/Payments interfaces.
    /// Payer records are managed directly in the database — no CRUD via API.
    /// </summary>
    public class PayerResponseDto
    {
        public int PayerID { get; set; }
        public required string PayerName { get; set; }
    }
}
 
 