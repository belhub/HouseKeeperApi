using HouseKeeperApi.Models;
using HouseKeeperApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseKeeperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // walidacja
    [Authorize(Roles = "Tenant, Landlord")] //autoryzacja
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("byIssueId/{issueId}")]
        public async Task<ActionResult<List<MessageDto>>> GetMessagesByIssueId([FromRoute] int issueId)
        {
            var messages = await _messageService.GetAllMessagesByIssueId(issueId);
            return messages == null ? NotFound() : Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessageForIssue([FromBody] MessageDto messageDto)
        {
            var messageId = await _messageService.CreateMessage(messageDto);
            return Created($"Pomyslnie utworzono wiadomosc o id: {messageId}", null);
        }
    }
}
