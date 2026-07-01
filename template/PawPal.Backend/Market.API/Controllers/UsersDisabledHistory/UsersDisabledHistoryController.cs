using PawPal.Application.Modules.Users.Commands.Create;
using PawPal.Application.Modules.Users.Commands.Delete;
using PawPal.Application.Modules.Users.Commands.ResendConfirmationEmail;
using PawPal.Application.Modules.Users.Commands.Update;
using PawPal.Application.Modules.Users.Commands.UpdatePassword;
using PawPal.Application.Modules.Users.Queries.GetByEmail;
using PawPal.Application.Modules.Users.Queries.GetById;
using PawPal.Application.Modules.Users.Queries.GetByUsername;
using PawPal.Application.Modules.Users.Queries.List;
using PawPal.Application.Modules.UsersDisabledHistory.Command.Create;
using PawPal.Application.Modules.UsersDisabledHistory.Command.Delete;

namespace PawPal.API.Controllers.UsersDisabledHistory
{

    [ApiController]
    [Route("[controller]")]
    public class UsersDisabledHistoryController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreateUserDisabled(CreateUserDisabledHistory cuc, CancellationToken ct)
        {
            int id = await sender.Send(cuc, ct);
            return id;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> DeleteUserDisable(int id, CancellationToken ct)
        {
            int result = await sender.Send(new DeleteUserDisabledCommand { UserID = id }, ct);
            return result;
        }
    }
}
