using System.ComponentModel.DataAnnotations;
 
namespace TeleCare.Model
{
    public class Rule
    {
        [Key]
        public int RuleID { get; set; }
 
        public required string Name { get; set; }
 
        public string? Description { get; set; }
        public DateTime? ActiveFrom { get; set; }
 
        public DateTime? ActiveTo { get; set; }
 
        public required string Status { get; set; }
    }
}
 
 