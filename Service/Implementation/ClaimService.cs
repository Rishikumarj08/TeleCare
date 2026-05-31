namespace TeleCare.Service.Implementation
{
    using TeleCare.Constants;
    using TeleCare.DTO;
    using TeleCare.Exceptions;
    using TeleCare.Model;
    using TeleCare.Repository.Interface;
    using TeleCare.Service.Interface;
 
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPayerRepository _payerRepository;
 
        public ClaimService(IClaimRepository claimRepository, IUserRepository userRepository, IPayerRepository payerRepository)
        {
            _claimRepository = claimRepository;
            _userRepository = userRepository;
            _payerRepository = payerRepository;
        }
 
        public async Task<List<ClaimResponseDto>> GetAllClaimsAsync()
        {
            var claims = await _claimRepository.GetAllClaimsAsync();
            return claims.Select(Map).ToList();
        }
 
        public async Task<ClaimResponseDto> GetClaimByIdAsync(int claimId)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim == null)
                throw new NotFoundException(AppConstants.ClaimNotFound);
            return Map(claim);
        }
 
        public async Task<List<ClaimResponseDto>> SearchClaimsAsync(SearchClaimDto searchDto)
        {
            var claims = await _claimRepository.SearchClaimsAsync(searchDto);
            if (claims == null || claims.Count == 0)
                throw new NotFoundException(AppConstants.NoClaimsFound);
            return claims.Select(Map).ToList();
        }
 
        public async Task CreateClaimAsync(ClaimCreateDto claimDto)
        {
            var patient = await _userRepository.GetUserByIdAsync(claimDto.PatientID);
            if (patient == null)
                throw new NotFoundException(AppConstants.PatientNotFound);
 
            var payer = await _payerRepository.GetPayerByIdAsync(claimDto.PayerID);
            if (payer == null)
                throw new NotFoundException(AppConstants.PayerNotFound);
 
            var claim = new Claim
            {
                PatientID = claimDto.PatientID,
                PayerID = claimDto.PayerID,
                SubmittedAt = claimDto.SubmittedAt,
                AmountBilled = claimDto.AmountBilled,
                AmountPaid = claimDto.AmountPaid,
                Status = claimDto.Status
            };
 
            await _claimRepository.AddClaimAsync(claim);
        }
 
        public async Task UpdateClaimAsync(int claimId, ClaimCreateDto claimDto)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim == null)
                throw new NotFoundException(AppConstants.ClaimNotFound);
 
            var patient = await _userRepository.GetUserByIdAsync(claimDto.PatientID);
            if (patient == null)
                throw new NotFoundException(AppConstants.PatientNotFound);
 
            var payer = await _payerRepository.GetPayerByIdAsync(claimDto.PayerID);
            if (payer == null)
                throw new NotFoundException(AppConstants.PayerNotFound);
 
            claim.PatientID = claimDto.PatientID;
            claim.PayerID = claimDto.PayerID;
            claim.SubmittedAt = claimDto.SubmittedAt;
            claim.AmountBilled = claimDto.AmountBilled;
            claim.AmountPaid = claimDto.AmountPaid;
            claim.Status = claimDto.Status;
 
            await _claimRepository.UpdateClaimAsync(claim);
        }
 
        public async Task DeleteClaimAsync(int claimId)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim == null)
                throw new NotFoundException(AppConstants.ClaimNotFound);
            await _claimRepository.DeleteClaimAsync(claim);
        }
 
        private static ClaimResponseDto Map(Claim claim) => new()
        {
            ClaimID = claim.ClaimID,
            PatientID = claim.PatientID,
            PatientName = claim.Patient?.Name ?? string.Empty,
            PayerID = claim.PayerID,
            PayerName = claim.Payer?.PayerName ?? string.Empty,
            SubmittedAt = claim.SubmittedAt,
            AmountBilled = claim.AmountBilled,
            AmountPaid = claim.AmountPaid,
            Status = claim.Status
        };
    }
}
 
 