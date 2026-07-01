using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List;
using PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Delete;
using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById;
using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List;
using PawPal.Application.Modules.Posts.Queries.GetByID;
using PawPal.Application.Modules.Users.Queries.GetById;

namespace PawPal.API.Controllers.Moderation
{
    [ApiController]
    [Route("[controller]")]
    public class ReportedUsersController(ISender sender) : ControllerBase
    {

        [HttpGet]
        public async Task<PageResult<ListUserReportsQueryDto>> List([FromQuery]
        ListUserReportsQuery query, CancellationToken tk)
        {
            var result = await sender.Send(query, tk);
            return result;
        }
        [HttpGet("{id:int}")]

        public async Task<GetUserReportByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await sender.Send(new GetUserReportByIdQuery { Id = id }, cancellationToken);
            return user;
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateUserReport(CreateReportUsersCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return id;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Unit>> DeleteUserReport(int id, CancellationToken ct)
        {
            return await sender.Send(new DeleteUserReportCommand { Id = id}, ct);
        }

    }
}
