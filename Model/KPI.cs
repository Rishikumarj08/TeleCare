using System.ComponentModel.DataAnnotations;

namespace TeleCare.Model
{
    public class KPI
    {
        [Key]
        public int KPIID { get; set; }
        public required string Name { get; set; }
        public string? Definition { get; set; }
        public decimal TargetValue { get; set; }
        public required string ReportingPeriod { get; set; }
    }
}
