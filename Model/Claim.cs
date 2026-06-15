using System.ComponentModel.DataAnnotations;
 
namespace TeleCare.Model
{
    public class Claim
    {
        [Key]
        public int ClaimID { get; set; }
 
        public int PatientID { get; set; }
 
        public int PayerID { get; set; }
 
        public DateTime SubmittedAt { get; set; }
 
        public decimal AmountBilled { get; set; }
 
        public decimal AmountPaid { get; set; }
 
        public required string Status { get; set; }
 
        public User? Patient { get; set; }
 
        public Payer? Payer { get; set; }
    }
}
 
 