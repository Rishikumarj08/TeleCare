namespace TeleCare.Controllers;
 
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using TeleCare.Constants;

using TeleCare.Service.Interface;
 
[Route("api/user/notifications")]

[ApiController]

[Authorize]

public class UserNotificationController : ControllerBase

{

    private readonly INotificationService _notificationService;
 
    public UserNotificationController(INotificationService notificationService)

    {

        _notificationService = notificationService;

    }
 
    

    [HttpGet("{userId}")]

    public async Task<IActionResult> GetMyNotifications(int userId)

    {

        var result = await _notificationService.GetNotificationsForUserAsync(userId);

        return Ok(result);

    }
 
    

    [HttpPatch("{notificationId}/read")]

    public async Task<IActionResult> MarkAsRead(int notificationId)

    {

        await _notificationService.MarkAsReadAsync(notificationId);

        return Ok(AppConstants.RecordUpdated);

    }

}
 