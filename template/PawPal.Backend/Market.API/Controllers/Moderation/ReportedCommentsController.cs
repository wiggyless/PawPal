using PawPal.Application.Modules.Moderation.ReportedComments.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedComments.Commands.Delete;
using PawPal.Application.Modules.Moderation.ReportedComments.Queries.GetById;
using PawPal.Application.Modules.Moderation.ReportedComments.Queries.List;
using PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Delete;
using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById;
using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.List;

namespace PawPal.API.Controllers.Moderation
{
    [ApiController]
    [Route("[controller]")]
    public class ReportedCommentsController
    (ISender sender) : ControllerBase
    {

        [HttpGet]
        public async Task<PageResult<ListCommentReportsQueryDto>> List([FromQuery]
        ListCommentReportsQuery query, CancellationToken tk)
        {
            var result = await sender.Send(query, tk);
            return result;
        }
        [HttpGet("{id:int}")]

        public async Task<GetCommentReportByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await sender.Send(new GetCommentReportByIdQuery { Id = id }, cancellationToken);
            return user;
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateCommentReport(CreateCommentReportCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return id;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Unit>> DeleteCommentReport(int id, CancellationToken ct)
        {
            return await sender.Send(new DeleteCommentReportCommand { Id = id }, ct);
        }

    }
}
