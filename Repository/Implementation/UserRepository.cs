namespace TeleCare.Repository.Implementation;

using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.DTO;
using TeleCare.Model;
using TeleCare.Repository.Interface;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Role)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserID == userId);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());
    }

    public async Task<User?> GetUserByPasswordResetTokenAsync(string token)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.PasswordResetToken == token);
    }

    public async Task<List<User>> GetUsersAsync(SearchUserDto searchDto)
    {
        var query = _context.Users
            .Include(u => u.Role)
            .AsQueryable();

        if (searchDto.UserID.HasValue)
            query = query.Where(u => u.UserID == searchDto.UserID.Value);

        if (!string.IsNullOrWhiteSpace(searchDto.Name))
            query = query.Where(u => u.Name.Contains(searchDto.Name));

        if (searchDto.RoleID.HasValue)
            query = query.Where(u => u.RoleID == searchDto.RoleID.Value);
        
        if (!string.IsNullOrWhiteSpace(searchDto.RoleName))
        {
            var normalizedRoleName = searchDto.RoleName.Trim().ToLower();
            query = query.Where(u => u.Role != null && u.Role.RoleName != null && u.Role.RoleName.ToLower() == normalizedRoleName);
        }

        return await query.ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}