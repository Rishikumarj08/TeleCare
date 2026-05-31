namespace TeleCare.DTO
{
    public class RuleCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public required string Status { get; set; }
    }
 
    public class RuleResponseDto
    {
        public int RuleID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public required string Status { get; set; }
    }
 
    public class SearchRuleDto
    {
        public int? RuleID { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
    }
}
 
 