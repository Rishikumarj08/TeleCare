namespace TeleCare.Service.Interface;

using TeleCare.DTO;

public interface IUserService
{
    Task<List<UserResponseDto>> GetAllUsersAsync();

    Task<UserResponseDto> GetUserByIdAsync(int userId);

    Task<List<UserResponseDto>> GetUsersAsync(SearchUserDto searchDto);

    Task CreateUserAsync(UserCreateDto userDto);

    Task UpdateUserAsync(int userId, UserCreateDto userDto);
}