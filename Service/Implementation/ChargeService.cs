namespace TeleCare.Service.Implementation
{
    using TeleCare.Constants;
    using TeleCare.DTO;
    using TeleCare.Exceptions;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
    using TeleCare.Service.Interface;
    public class ChargeService : IChargeService
    {
        private readonly IChargeRepository _chargeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogService _auditLogService;

        public ChargeService(IChargeRepository chargeRepository, IUserRepository userRepository,
            IAuditLogService auditLogService)
        {
            _chargeRepository = chargeRepository;
            _userRepository = userRepository;
            _auditLogService = auditLogService;
        }

        public async Task<List<ChargeResponseDto>> GetAllChargesAsync()
        {
            var charges = await _chargeRepository.GetAllChargesAsync();
            return charges.Select(Map).ToList();
        }
        public async Task<ChargeResponseDto> GetChargeByIdAsync(int chargeId)
        {
            var charge = await _chargeRepository.GetChargeByIdAsync(chargeId);
            if (charge == null)
                throw new NotFoundException(AppConstants.ChargeNotFound);
            return Map(charge);
        }
        public async Task<List<ChargeResponseDto>> SearchChargesAsync(SearchChargeDto searchDto)
        {
            var charges = await _chargeRepository.SearchChargesAsync(searchDto);
            if (charges == null || charges.Count == 0)
                throw new NotFoundException(AppConstants.NoChargesFound);
            return charges.Select(Map).ToList();
        }
        public async Task CreateChargeAsync(ChargeCreateDto chargeDto)
        {
            var patient = await _userRepository.GetUserByIdAsync(chargeDto.PatientID);
            if (patient == null)
                throw new NotFoundException(AppConstants.PatientNotFound);
            var charge = new Charge
            {
                PatientID = chargeDto.PatientID,
                Amount = chargeDto.Amount,
                Date = chargeDto.Date,
                Status = chargeDto.Status
            };

            await _chargeRepository.AddChargeAsync(charge);
            await _auditLogService.LogAsync(chargeDto.PatientID, "CREATE", "Charge", charge.ChargeID,
                $"Charge of '{chargeDto.Amount}' created for patient '{patient.Name}' on '{chargeDto.Date:yyyy-MM-dd}'.");
        }

        public async Task UpdateChargeAsync(int chargeId, ChargeCreateDto chargeDto)
        {
            var charge = await _chargeRepository.GetChargeByIdAsync(chargeId);
            if (charge == null)
                throw new NotFoundException(AppConstants.ChargeNotFound);

            var patient = await _userRepository.GetUserByIdAsync(chargeDto.PatientID);
            if (patient == null)
                throw new NotFoundException(AppConstants.PatientNotFound);

            charge.PatientID = chargeDto.PatientID;
            charge.Amount = chargeDto.Amount;
            charge.Date = chargeDto.Date;
            charge.Status = chargeDto.Status;

            await _chargeRepository.UpdateChargeAsync(charge);
            await _auditLogService.LogAsync(chargeDto.PatientID, "UPDATE", "Charge", chargeId,
                $"Charge '{chargeId}' updated for patient '{patient.Name}'. Status: '{chargeDto.Status}'.");
        }

        public async Task DeleteChargeAsync(int chargeId)
        {
            var charge = await _chargeRepository.GetChargeByIdAsync(chargeId);
            if (charge == null)
                throw new NotFoundException(AppConstants.ChargeNotFound);

            var patientId = charge.PatientID;
            var patientName = charge.Patient?.Name ?? string.Empty;
            await _chargeRepository.DeleteChargeAsync(charge);
            await _auditLogService.LogAsync(patientId, "DELETE", "Charge", chargeId,
                $"Charge '{chargeId}' deleted for patient '{patientName}'.");
        }

        private static ChargeResponseDto Map(Charge charge) => new()
        {
            ChargeID = charge.ChargeID,
            PatientID = charge.PatientID,
            PatientName = charge.Patient?.Name ?? string.Empty,
            Amount = charge.Amount,
            Date = charge.Date,
            Status = charge.Status
        };
    }
}
