namespace TeleCare.DTO
{
    public class NotificationSendDto
    {
        public int UserID { get; set; }
        public required string Message { get; set; }
        public required string Category { get; set; }
    }
 
    public class NotificationResponseDto
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public required string Message { get; set; }
        public required string Category { get; set; }
        public required string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
 
 