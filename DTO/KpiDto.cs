namespace TeleCare.DTO
{
    public class KpiCreateDto
    {
        public required string Name { get; set; }
        public string? Definition { get; set; }
        public decimal TargetValue { get; set; }
        public required string ReportingPeriod { get; set; }
    }

    public class KpiResponseDto
    {
        public required string Name { get; set; }
        public string? Definition { get; set; }
        public decimal TargetValue { get; set; }
        public decimal CurrentValue { get; set; }
        public required string ReportingPeriod { get; set; }
        public required string PerformanceIndicator { get; set; }
    }

    public class SearchKpiDto
    {
        public string? Name { get; set; }
        public string? ReportingPeriod { get; set; }
        public string? PerformanceIndicator { get; set; }
    }
}
