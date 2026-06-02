using System.ComponentModel.DataAnnotations;
 
namespace TeleCare.Model
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
 
        public int UserID { get; set; }
 
        public required string Message { get; set; }
 
        public required string Category { get; set; }
 
        public required string Status { get; set; }
 
        public DateTime CreatedAt { get; set; }
 
        // Navigation property
        public User? User { get; set; }
    }
}
 
 