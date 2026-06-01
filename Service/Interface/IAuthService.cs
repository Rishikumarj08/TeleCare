namespace TeleCare.Service.Interface;

using TeleCare.DTO;

public interface IAuthService
{
    Task RegisterAsync(AuthRegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(AuthLoginDto loginDto);
    Task<AuthResponseDto> VerifyClinicianPinAsync(AuthPinVerificationDto pinDto);
    Task ForgotPasswordAsync(ForgotPasswordRequestDto requestDto);
    Task ResetPasswordAsync(ResetPasswordDto resetDto);
}
