namespace TeleCare.DTO
{
    public class UserCreateDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Password { get; set; }
        public required string RoleName { get; set; }
        public string? Pin { get; set; }
    }

    public class UserResponseDto
    {
        public int UserID { get; set; }
        public required string Name { get; set; }
        public required string RoleName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public int RoleID { get; set; }
        public bool MFAEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SearchUserDto
    {
        public int? UserID { get; set; }
        public string? Name { get; set; }
        public int? RoleID { get; set; }
        public string? RoleName { get; set; }
    }
}
