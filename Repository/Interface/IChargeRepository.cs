namespace TeleCare.Repository.Interface
{
    using TeleCare.DTO;
    using TeleCare.Model;
 
    public interface IChargeRepository
    {
        Task<List<Charge>> GetAllChargesAsync();
        Task<Charge?> GetChargeByIdAsync(int chargeId);
        Task<List<Charge>> SearchChargesAsync(SearchChargeDto searchDto);
        Task AddChargeAsync(Charge charge);
        Task UpdateChargeAsync(Charge charge);
        Task DeleteChargeAsync(Charge charge);
    }
}
 
 