using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Tenant, Landlord")] //autoryzacja

    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<NotificationDto>> GetNotificationsForUser([FromRoute] int userId)
        {
            var notifications = await _notificationService.GetNotificationsForUserId(userId);

            return notifications != null ? Ok(notifications) : NotFound();
        }

        [HttpPut]
        public async Task<ActionResult<NotificationDto>> EditNotification([FromBody] NotificationDto notificationDto)
        {
            var notifications = await _notificationService.UpdateNotification(notificationDto);

            return notifications ? Ok() : NotFound();
        }
    }
}
