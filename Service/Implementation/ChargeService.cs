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
 
        public ChargeService(IChargeRepository chargeRepository, IUserRepository userRepository)
        {
            _chargeRepository = chargeRepository;
            _userRepository = userRepository;
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
        }
 
        public async Task DeleteChargeAsync(int chargeId)
        {
            var charge = await _chargeRepository.GetChargeByIdAsync(chargeId);
            if (charge == null)
                throw new NotFoundException(AppConstants.ChargeNotFound);
            await _chargeRepository.DeleteChargeAsync(charge);
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
 
 