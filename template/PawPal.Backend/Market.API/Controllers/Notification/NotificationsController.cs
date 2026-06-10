using PawPal.Application.Modules.Notifications.Commands.RegisterFcmToken;

namespace PawPal.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class NotificationsController(ISender sender) : ControllerBase
{
    [HttpPost("register-token")]
    public async Task<IActionResult> RegisterToken([FromBody] RegisterFcmTokenCommand command, CancellationToken ct)
    {
        await sender.Send(command, ct);
        return Ok();
    }
}