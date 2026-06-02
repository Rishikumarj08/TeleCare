namespace TeleCare.Service.Implementation;

using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Enum;
using TeleCare.Exceptions;
using TeleCare.Helpers;
using TeleCare.Model;
using TeleCare.Repository.Interface;
using TeleCare.Service.Interface;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogService _auditLogService;

    public UserService(IUserRepository userRepository, IAuditLogService auditLogService)
    {
        _userRepository = userRepository;
        _auditLogService = auditLogService;
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(Map).ToList();
    }

    public async Task<UserResponseDto> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
            throw new NotFoundException(AppConstants.RecordNotFound);

        return Map(user);
    }

    public async Task<List<UserResponseDto>> GetUsersAsync(SearchUserDto searchDto)
    {
        var users = await _userRepository.GetUsersAsync(searchDto);

        if (users == null || users.Count == 0)
            throw new NotFoundException(AppConstants.NoUsersFound);

        return users.Select(Map).ToList();
    }

    public async Task CreateUserAsync(UserCreateDto userDto)
    {
        var role = GetNormalizedRole(userDto.RoleName);
        if (role == null)
            throw new BadRequestException(AppConstants.InvalidRole);

        if (role.Value == RoleEnum.Clinician && string.IsNullOrWhiteSpace(userDto.Pin))
            throw new BadRequestException("Clinician accounts require a PIN.");
        if (role.Value != RoleEnum.Clinician && !string.IsNullOrWhiteSpace(userDto.Pin))
            throw new BadRequestException("PIN should only be supplied for clinician accounts.");

        var user = new User
        {
            Name = userDto.Name,
            RoleID = (int)role.Value,
            Email = userDto.Email,
            Phone = userDto.Phone,
            PasswordHash = PasswordHasher.Hash(userDto.Password),
            Pin = role.Value == RoleEnum.Clinician ? PasswordHasher.Hash(userDto.Pin!) : null,
            MFAEnabled = role.Value == RoleEnum.Clinician,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user);
        await _auditLogService.LogAsync(user.UserID, "CREATE", "User", user.UserID,
            $"User '{user.Name}' created with role '{userDto.RoleName}'.");
    }

    public async Task UpdateUserAsync(int userId, UserCreateDto userDto)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
            throw new NotFoundException(AppConstants.RecordNotFound);

        user.Name = userDto.Name;
        var role = GetNormalizedRole(userDto.RoleName);
        if (role == null)
            throw new BadRequestException(AppConstants.InvalidRole);
        if (role.Value == RoleEnum.Clinician && string.IsNullOrWhiteSpace(userDto.Pin))
            throw new BadRequestException("Clinician accounts require a PIN.");
        if (role.Value != RoleEnum.Clinician && !string.IsNullOrWhiteSpace(userDto.Pin))
            throw new BadRequestException("PIN should only be supplied for clinician accounts.");
        user.RoleID = (int)role.Value;
        user.Email = userDto.Email;
        user.Phone = userDto.Phone;
        user.PasswordHash = PasswordHasher.Hash(userDto.Password);
        user.Pin = role.Value == RoleEnum.Clinician ? PasswordHasher.Hash(userDto.Pin!) : null;
        user.MFAEnabled = role.Value == RoleEnum.Clinician;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateUserAsync(user);
        await _auditLogService.LogAsync(userId, "UPDATE", "User", userId,
            $"User '{user.Name}' updated. Role: '{userDto.RoleName}'.");
    }

    private UserResponseDto Map(User user)
    {
        return new UserResponseDto
        {
            UserID = user.UserID,
            Name = user.Name,
            RoleName = user.Role?.RoleName ?? GetRoleDisplayName(user.RoleID),
            RoleID = user.RoleID,
            MFAEnabled = user.MFAEnabled,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            Email = user.Email,
            Phone = user.Phone
        };
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

    private static string GetRoleDisplayName(int roleId)
    {
        return roleId switch
        {
            (int)RoleEnum.Patient => "Patient",
            (int)RoleEnum.Clinician => "Clinician",
            (int)RoleEnum.CareCoordinator => "Care Coordinator",
            (int)RoleEnum.DeviceTechnician => "Device Technician",
            (int)RoleEnum.Administrator => "Administrator",
            (int)RoleEnum.Auditor => "Auditor",
            _ => string.Empty
        };
    }
}