namespace TeleCare.DTO;

public class AuthRegisterDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Password { get; set; }
    public required string RoleName { get; set; }
    public string? Pin { get; set; }
}

public class AuthLoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class AuthPinVerificationDto
{
    public required string Email { get; set; }
    public required string Pin { get; set; }
}

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public bool RequiresPin { get; set; }
    public string? Message { get; set; }
}

public class ForgotPasswordRequestDto
{
    public required string Email { get; set; }
}

public class ResetPasswordDto
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }
}
