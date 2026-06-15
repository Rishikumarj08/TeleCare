namespace TeleCare.Service.Implementation
{
    using Microsoft.EntityFrameworkCore;
    using TeleCare.Constants;
    using TeleCare.Data;
    using TeleCare.DTO;
    using TeleCare.Enum;
    using TeleCare.Exceptions;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
    using TeleCare.Service.Interface;

    public class KpiService : IKpiService
    {
        private readonly IKpiRepository _kpiRepository;
        private readonly AppDbContext _context;

        public KpiService(IKpiRepository kpiRepository, AppDbContext context)
        {
            _kpiRepository = kpiRepository;
            _context = context;
        }

        public async Task<List<KpiResponseDto>> GetAllKpisAsync()
        {
            var kpis = await _kpiRepository.GetAllKpisAsync();
            if (kpis == null || kpis.Count == 0)
                throw new NotFoundException(AppConstants.NoKpisFound);

            var result = new List<KpiResponseDto>();
            foreach (var kpi in kpis)
            {
                var currentValue = await CalculateCurrentValueAsync(kpi.Name);
                result.Add(MapWithCalculation(kpi, currentValue));
            }
            return result;
        }

        public async Task<KpiResponseDto> GetKpiByIdAsync(int kpiId)
        {
            var kpi = await _kpiRepository.GetKpiByIdAsync(kpiId);
            if (kpi == null)
                throw new NotFoundException(AppConstants.KpiNotFound);

            var currentValue = await CalculateCurrentValueAsync(kpi.Name);
            return MapWithCalculation(kpi, currentValue);
        }

        public async Task<List<KpiResponseDto>> SearchKpisAsync(SearchKpiDto searchDto)
        {
            var kpis = await _kpiRepository.GetAllKpisAsync();
            if (kpis == null || kpis.Count == 0)
                throw new NotFoundException(AppConstants.NoKpisFound);

            var result = new List<KpiResponseDto>();
            foreach (var kpi in kpis)
            {
                var currentValue = await CalculateCurrentValueAsync(kpi.Name);
                result.Add(MapWithCalculation(kpi, currentValue));
            }

            // Apply search filters
            if (!string.IsNullOrWhiteSpace(searchDto.Name))
                result = result.Where(k => k.Name.Contains(searchDto.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(searchDto.ReportingPeriod))
                result = result.Where(k => k.ReportingPeriod.ToLower() == searchDto.ReportingPeriod.Trim().ToLower()).ToList();

            if (!string.IsNullOrWhiteSpace(searchDto.PerformanceIndicator))
                result = result.Where(k => k.PerformanceIndicator.ToLower() == searchDto.PerformanceIndicator.Trim().ToLower()).ToList();

            if (result.Count == 0)
                throw new NotFoundException(AppConstants.NoKpisFound);

            return result;
        }

        public async Task CreateKpiAsync(KpiCreateDto kpiDto)
        {
            if (string.IsNullOrWhiteSpace(kpiDto.Name))
                throw new BadRequestException(AppConstants.KpiNameRequired);

            if (string.IsNullOrWhiteSpace(kpiDto.ReportingPeriod))
                throw new BadRequestException(AppConstants.KpiReportingPeriodRequired);

            if (kpiDto.TargetValue < 0 || kpiDto.TargetValue > 100)
                throw new BadRequestException(AppConstants.KpiTargetValueInvalid);

            var kpi = new KPI
            {
                Name = kpiDto.Name,
                Definition = kpiDto.Definition,
                TargetValue = kpiDto.TargetValue,
                ReportingPeriod = kpiDto.ReportingPeriod
            };

            await _kpiRepository.AddKpiAsync(kpi);
        }

        public async Task UpdateKpiAsync(int kpiId, KpiCreateDto kpiDto)
        {
            var kpi = await _kpiRepository.GetKpiByIdAsync(kpiId);
            if (kpi == null)
                throw new NotFoundException(AppConstants.KpiNotFound);

            if (string.IsNullOrWhiteSpace(kpiDto.Name))
                throw new BadRequestException(AppConstants.KpiNameRequired);

            if (string.IsNullOrWhiteSpace(kpiDto.ReportingPeriod))
                throw new BadRequestException(AppConstants.KpiReportingPeriodRequired);

            if (kpiDto.TargetValue < 0 || kpiDto.TargetValue > 100)
                throw new BadRequestException(AppConstants.KpiTargetValueInvalid);

            kpi.Name = kpiDto.Name;
            kpi.Definition = kpiDto.Definition;
            kpi.TargetValue = kpiDto.TargetValue;
            kpi.ReportingPeriod = kpiDto.ReportingPeriod;

            await _kpiRepository.UpdateKpiAsync(kpi);
        }

        public async Task DeleteKpiAsync(int kpiId)
        {
            var kpi = await _kpiRepository.GetKpiByIdAsync(kpiId);
            if (kpi == null)
                throw new NotFoundException(AppConstants.KpiNotFound);
            await _kpiRepository.DeleteKpiAsync(kpi);
        }

        private async Task<decimal> CalculateCurrentValueAsync(string kpiName)
        {
            switch (kpiName.Trim().ToLower())
            {
                case "claim approval rate":
                    var totalClaims = await _context.Claims.CountAsync();
                    var approvedClaims = await _context.Claims.CountAsync(c => c.Status.ToLower() == "approved");
                    return totalClaims == 0 ? 0 : Math.Round((decimal)approvedClaims / totalClaims * 100, 2);

                case "claim rejection rate":
                    var totalClaims2 = await _context.Claims.CountAsync();
                    var rejectedClaims = await _context.Claims.CountAsync(c => c.Status.ToLower() == "rejected");
                    return totalClaims2 == 0 ? 0 : Math.Round((decimal)rejectedClaims / totalClaims2 * 100, 2);

                case "payment collection rate":
                    var totalPayments = await _context.Payments.CountAsync();
                    var completedPayments = await _context.Payments.CountAsync(p => p.Status.ToLower() == "completed");
                    return totalPayments == 0 ? 0 : Math.Round((decimal)completedPayments / totalPayments * 100, 2);

                case "active rules count":
                    var totalRules = await _context.Rules.CountAsync();
                    var activeRules = await _context.Rules.CountAsync(r => r.Status.ToLower() == "active");
                    return totalRules == 0 ? 0 : Math.Round((decimal)activeRules / totalRules * 100, 2);

                case "notification read rate":
                    var totalNotifications = await _context.Notifications.CountAsync();
                    var readNotifications = await _context.Notifications.CountAsync(n => n.Status.ToLower() == "read");
                    return totalNotifications == 0 ? 0 : Math.Round((decimal)readNotifications / totalNotifications * 100, 2);

                case "appointment completion rate":
                    var totalAppointments = await _context.Appointments.CountAsync();
                    var completedAppointments = await _context.Appointments.CountAsync(a => a.Status != null && a.Status.Trim().ToLower() == "completed");
                    return totalAppointments == 0 ? 0 : Math.Round((decimal)completedAppointments / totalAppointments * 100, 2);

                case "alert resolution rate":
                    var totalAlerts = await _context.Alerts.CountAsync();
                    var resolvedAlerts = await _context.Alerts.CountAsync(a => a.Status != null && a.Status.Trim().ToLower() == "resolved");
                    return totalAlerts == 0 ? 0 : Math.Round((decimal)resolvedAlerts / totalAlerts * 100, 2);

                case "enrollment active rate":
                    var totalEnrollments = await _context.Enrollments.CountAsync();
                    var activeEnrollments = await _context.Enrollments.CountAsync(e => e.Status == EnrollmentStatus.Active);
                    return totalEnrollments == 0 ? 0 : Math.Round((decimal)activeEnrollments / totalEnrollments * 100, 2);

                case "medication adherence rate":
                    var totalAdherence = await _context.AdherenceRecords.CountAsync();
                    var takenAdherence = await _context.AdherenceRecords.CountAsync(a => a.Status == AdherenceStatus.Taken);
                    return totalAdherence == 0 ? 0 : Math.Round((decimal)takenAdherence / totalAdherence * 100, 2);

                case "device assignment rate":
                    var totalDevices = await _context.Devices.CountAsync();
                    var assignedDevices = await _context.Devices.CountAsync(d => d.AssignedToPatientID != null);
                    return totalDevices == 0 ? 0 : Math.Round((decimal)assignedDevices / totalDevices * 100, 2);

                default:
                    return 0;
            }
        }

        private static string GetPerformanceIndicator(decimal currentValue, decimal targetValue)
        {
            if (currentValue >= targetValue)
                return KpiPerformanceIndicator.Exceeded.ToString();
            else if (currentValue >= targetValue - 10)
                return KpiPerformanceIndicator.OnTrack.ToString();
            else
                return KpiPerformanceIndicator.BelowTarget.ToString();
        }

        private static KpiResponseDto MapWithCalculation(KPI kpi, decimal currentValue) => new()
        {
            Name = kpi.Name,
            Definition = kpi.Definition,
            TargetValue = kpi.TargetValue,
            CurrentValue = currentValue,
            ReportingPeriod = kpi.ReportingPeriod,
            PerformanceIndicator = GetPerformanceIndicator(currentValue, kpi.TargetValue)
        };
    }
}
