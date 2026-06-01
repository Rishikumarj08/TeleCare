namespace TeleCare.Service.Interface
{
    using TeleCare.DTO;
 
    public interface IClaimService
    {
        Task<List<ClaimResponseDto>> GetAllClaimsAsync();
        Task<ClaimResponseDto> GetClaimByIdAsync(int claimId);
        Task<List<ClaimResponseDto>> SearchClaimsAsync(SearchClaimDto searchDto);
        Task CreateClaimAsync(ClaimCreateDto claimDto);
        Task UpdateClaimAsync(int claimId, ClaimCreateDto claimDto);
        Task DeleteClaimAsync(int claimId);
    }
}