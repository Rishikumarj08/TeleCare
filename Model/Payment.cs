using System.ComponentModel.DataAnnotations;
 
namespace TeleCare.Model
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
 
        public int ClaimID { get; set; }
 
        public decimal Amount { get; set; }
 
        public required string Method { get; set; }
 
        public DateTime DatePaid { get; set; }
 
        public required string Status { get; set; }
 
        public Claim? Claim { get; set; } 
    }
}
 
 