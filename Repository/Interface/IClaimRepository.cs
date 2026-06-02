namespace TeleCare.Repository.Interface
{
    using TeleCare.DTO;
    using TeleCare.Model;
 
    public interface IClaimRepository
    {
        Task<List<Claim>> GetAllClaimsAsync();
        Task<Claim?> GetClaimByIdAsync(int claimId);
        Task<List<Claim>> SearchClaimsAsync(SearchClaimDto searchDto);
        Task AddClaimAsync(Claim claim);
        Task UpdateClaimAsync(Claim claim);
        Task DeleteClaimAsync(Claim claim);
    }
}
 
 