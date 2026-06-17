using PawPal.Application.Common;
using PawPal.Application.Modules.Messaging.Commands.SendMessage;
using PawPal.Application.Modules.Messaging.Commands.StartConversation;
using PawPal.Application.Modules.Messaging.Dtos;
using PawPal.Application.Modules.Messaging.Queries.GetConversations;

namespace PawPal.API.Controllers.Messaging
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController(ISender sender) : ControllerBase
    {
        [HttpPost("send")]
        public async Task<ActionResult<MessageDto>> Send(SendMessageCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return result;
        }

        [HttpGet("conversations/{userId}")]
        public async Task<IActionResult> GetConversations(int userId, CancellationToken ct)
    => Ok(await sender.Send(new GetConversationsQuery { UserId = userId }, ct));

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(
     int conversationId,
     [FromQuery] int requestingUserId,
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10,
     CancellationToken ct = default)
        {
            var query = new GetMessagesQuery
            {
                ConversationId = conversationId,
                RequestingUserId = requestingUserId,
                Paging = new PageRequest { Page = page, PageSize = pageSize }
            };
            return Ok(await sender.Send(query, ct));
        }

        [HttpGet("conversation")]
        public async Task<IActionResult> GetOrCreate([FromQuery] int senderId, [FromQuery] int recipientId)
    => Ok(await sender.Send(new GetOrCreateConversationQuery
    {
        SenderId = senderId,
        RecipientId = recipientId
    }));
    }
}