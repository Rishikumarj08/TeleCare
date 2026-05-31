namespace TeleCare.Repository.Interface;

using TeleCare.DTO;
using TeleCare.Model;

public interface IUserRepository
{
    Task<List<User>> GetAllUsersAsync();

    Task<User?> GetUserByIdAsync(int userId);

    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByPasswordResetTokenAsync(string token);

    Task<List<User>> GetUsersAsync(SearchUserDto searchDto);

    Task AddUserAsync(User user);

    Task UpdateUserAsync(User user);
}