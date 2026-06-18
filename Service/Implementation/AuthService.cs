namespace TeleCare.Service.Implementation;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ModelClaim = TeleCare.Model.Claim;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Enum;
using TeleCare.Exceptions;
using TeleCare.Helpers;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task RegisterAsync(AuthRegisterDto registerDto)
    {
        var role = GetNormalizedRole(registerDto.RoleName);
        if (role == null || (role != RoleEnum.Patient && role != RoleEnum.Administrator))
            throw new BadRequestException(AppConstants.RegistrationRestricted);

        var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
        if (existingUser != null)
            throw new BadRequestException(AppConstants.EmailAlreadyRegistered);

        var user = new User
        {
            Name = registerDto.Name,
            Email = registerDto.Email,
            Phone = registerDto.Phone,
            RoleID = (int)role.Value,
            PasswordHash = PasswordHasher.Hash(registerDto.Password),
            MFAEnabled = role.Value == RoleEnum.Clinician,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (role == RoleEnum.Clinician)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Pin))
                throw new BadRequestException("Clinician registration requires a PIN.");

            user.Pin = PasswordHasher.Hash(registerDto.Pin);
        }

        await _userRepository.AddUserAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(AuthLoginDto loginDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null || !PasswordHasher.Verify(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedException(AppConstants.InvalidCredentials);

        if (user.MFAEnabled)
        {
            return new AuthResponseDto
            {
                Success = false,
                RequiresPin = true,
                Message = AppConstants.ClinicianPinRequired
            };
        }

        return new AuthResponseDto
        {
            Success = true,
            Token = GenerateJwtToken(user),
            Message = "Login successful."
        };
    }

    public async Task<AuthResponseDto> VerifyClinicianPinAsync(AuthPinVerificationDto pinDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(pinDto.Email);
        if (user == null || !user.MFAEnabled || string.IsNullOrWhiteSpace(user.Pin))
            throw new UnauthorizedException(AppConstants.InvalidCredentials);

        if (!PasswordHasher.Verify(pinDto.Pin, user.Pin))
            throw new UnauthorizedException(AppConstants.InvalidCredentials);

        return new AuthResponseDto
        {
            Success = true,
            Token = GenerateJwtToken(user),
            Message = "Clinician login successful."
        };
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestDto requestDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(requestDto.Email);
        if (user == null || user.RoleID != (int)RoleEnum.Patient)
            throw new BadRequestException(AppConstants.PatientForgotPasswordNotSupported);

        user.PasswordResetToken = Guid.NewGuid().ToString("N");
        user.PasswordResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateUserAsync(user);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto resetDto)
    {
        var user = await _userRepository.GetUserByEmailAsync(resetDto.Email);
        if (user == null || string.IsNullOrWhiteSpace(user.PasswordResetToken) ||
            user.PasswordResetToken != resetDto.Token ||
            user.PasswordResetTokenExpiresAt == null ||
            user.PasswordResetTokenExpiresAt < DateTime.UtcNow)
        {
            throw new BadRequestException(AppConstants.PasswordResetTokenInvalid);
        }

        user.PasswordHash = PasswordHasher.Hash(resetDto.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiresAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateUserAsync(user);
    }

    private string GenerateJwtToken(User user)
    {
        var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured.");
        var issuer = _configuration["Jwt:Issuer"] ?? "TeleCareApi";
        var audience = _configuration["Jwt:Audience"] ?? "TeleCareClient";
        var expiryMinutes = int.TryParse(_configuration["Jwt:TokenExpiryMinutes"], out var minutes) ? minutes : 60;
        var roleName = System.Enum.GetName(typeof(RoleEnum), user.RoleID) ?? string.Empty;
        var claims = new List<System.Security.Claims.Claim>
        {
            new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
            new System.Security.Claims.Claim(ClaimTypes.Email, user.Email),
            new System.Security.Claims.Claim(ClaimTypes.Role, roleName),
            new System.Security.Claims.Claim("role", roleName),
            new System.Security.Claims.Claim("name", user.Name)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private static RoleEnum? GetNormalizedRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return null;

        var normalized = roleName.Trim().Replace(" ", string.Empty).ToLowerInvariant();
        return normalized switch
        {
            "patient" => RoleEnum.Patient,
            "clinician" => RoleEnum.Clinician,
            "carecoordinator" => RoleEnum.CareCoordinator,
            "devicetechnician" => RoleEnum.DeviceTechnician,
            "administrator" => RoleEnum.Administrator,
            "auditor" => RoleEnum.Auditor,
            _ => null
        };
    }
}
