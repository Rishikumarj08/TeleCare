namespace TeleCare.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.DTO;
using TeleCare.Service.Interface;

[Route("api/auth")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _authService.RegisterAsync(registerDto);
        return Ok("Registration completed successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.LoginAsync(loginDto);
        return Ok(response);
    }

    [HttpPost("login/verify-pin")]
    public async Task<IActionResult> VerifyPin([FromBody] AuthPinVerificationDto pinDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.VerifyClinicianPinAsync(pinDto);
        return Ok(response);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto requestDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _authService.ForgotPasswordAsync(requestDto);
        return Ok("Password reset instructions have been generated for the patient account.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _authService.ResetPasswordAsync(resetDto);
        return Ok("Password has been reset successfully.");
    }
}
