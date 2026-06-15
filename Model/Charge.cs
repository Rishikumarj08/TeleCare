using System.ComponentModel.DataAnnotations;
 
namespace TeleCare.Model
{
    public class Charge
    {
        [Key]
        public int ChargeID { get; set; }
 
        public int PatientID { get; set; }
 
        public decimal Amount { get; set; }
 
        public DateTime Date { get; set; }
 
        public required string Status { get; set; }
 
        public User? Patient { get; set; }
    }
}
 
 