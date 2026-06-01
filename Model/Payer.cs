using System.ComponentModel.DataAnnotations;
 
namespace TeleCare.Model
{
    public class Payer
    {
        [Key]
        public int PayerID { get; set; }
 
        public required string PayerName { get; set; }
 
        public string? ContactEmail { get; set; }
 
        public string? ContactPhone { get; set; }
    }
}
 
 