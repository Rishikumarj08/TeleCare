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
        private readonly IAuditLogService _auditLogService;

        public ClaimService(IClaimRepository claimRepository, IUserRepository userRepository,
            IPayerRepository payerRepository, IAuditLogService auditLogService)
        {
            _claimRepository = claimRepository;
            _userRepository = userRepository;
            _payerRepository = payerRepository;
            _auditLogService = auditLogService;
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
            await _auditLogService.LogAsync(claimDto.PatientID, "CREATE", "Claim", claim.ClaimID,
                $"Claim created for patient '{patient.Name}' with payer '{payer.PayerName}'. Amount billed: {claimDto.AmountBilled}.");
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
            await _auditLogService.LogAsync(claimDto.PatientID, "UPDATE", "Claim", claimId,
                $"Claim updated for patient '{patient.Name}'. Status: '{claimDto.Status}'.");
        }

        public async Task DeleteClaimAsync(int claimId)
        {
            var claim = await _claimRepository.GetClaimByIdAsync(claimId);
            if (claim == null)
                throw new NotFoundException(AppConstants.ClaimNotFound);

            var patientId = claim.PatientID;
            var patientName = claim.Patient?.Name ?? string.Empty;
            await _claimRepository.DeleteClaimAsync(claim);
            await _auditLogService.LogAsync(patientId, "DELETE", "Claim", claimId,
                $"Claim deleted for patient '{patientName}'.");
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
