namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;
 
    public interface IChargeService
    {
        Task<List<ChargeResponseDto>> GetAllChargesAsync();
        Task<ChargeResponseDto> GetChargeByIdAsync(int chargeId);
        Task<List<ChargeResponseDto>> SearchChargesAsync(SearchChargeDto searchDto);
        Task CreateChargeAsync(ChargeCreateDto chargeDto);
        Task UpdateChargeAsync(int chargeId, ChargeCreateDto chargeDto);
        Task DeleteChargeAsync(int chargeId);
    }
}
 
 