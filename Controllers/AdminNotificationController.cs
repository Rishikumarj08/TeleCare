namespace TeleCare.Controllers;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeleCare.Constants;
using TeleCare.DTO;
using TeleCare.Service.Interface;
 
[Route("api/admin/notifications")]
[ApiController]
[Authorize(Roles = "Administrator")]
public class AdminNotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
 
    public AdminNotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }
 
    /// <summary>
    /// Get all notifications (bell icon list for the administrator)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllNotifications()
    {
        var result = await _notificationService.GetAllNotificationsAsync();
        return Ok(result);
    }
 
    /// <summary>
    /// Get all notifications for a specific user
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetNotificationsForUser(int userId)
    {
        var result = await _notificationService.GetNotificationsForUserAsync(userId);
        return Ok(result);
    }
 
    /// <summary>
    /// Send a notification to a user (requires UserID, Category, Message)
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationSendDto notificationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        await _notificationService.SendNotificationAsync(notificationDto);
        return Ok(AppConstants.RecordCreated);
    }
 
    /// <summary>
    /// Delete a notification
    /// </summary>
    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        await _notificationService.DeleteNotificationAsync(notificationId);
        return Ok(AppConstants.RecordDeleted);
    }
}
 
 