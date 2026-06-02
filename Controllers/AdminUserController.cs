namespace TeleCare.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Service.Interface;

[Route("api/admin/users")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminUserController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminUserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> GetUsers([FromBody] SearchUserDto searchDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.GetUsersAsync(searchDto);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.CreateUserAsync(userDto);

        return Ok(AppConstants.RecordCreated);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserCreateDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _userService.UpdateUserAsync(userId, userDto);

        return Ok(AppConstants.RecordUpdated);
    }
}