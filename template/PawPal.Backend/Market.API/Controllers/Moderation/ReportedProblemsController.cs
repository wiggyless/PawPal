using PawPal.Application.Modules.Moderation.ReportedComments.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedComments.Commands.Delete;
using PawPal.Application.Modules.Moderation.ReportedComments.Queries.GetById;
using PawPal.Application.Modules.Moderation.ReportedComments.Queries.List;
using PawPal.Application.Modules.Moderation.ReportedProblems.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedProblems.Commands.Delete;
using PawPal.Application.Modules.Moderation.ReportedProblems.Queries.GetById;
using PawPal.Application.Modules.Moderation.ReportedProblems.Queries.List;

namespace PawPal.API.Controllers.Moderation
{
    [ApiController]
    [Route("[controller]")]
    public class ReportedProblemsController(ISender sender) : ControllerBase
    {

        [HttpGet]
        public async Task<PageResult<ListProblemReportsQueryDto>> List([FromQuery]
        ListProblemReportsQuery query, CancellationToken tk)
        {
            var result = await sender.Send(query, tk);
            return result;
        }
        [HttpGet("{id:int}")]

        public async Task<GetProblemReportByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await sender.Send(new GetProblemReportById { Id = id }, cancellationToken);
            return user;
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateCommentReport(CreateProblemReportCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return id;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Unit>> DeleteCommentReport(int id, CancellationToken ct)
        {
            return await sender.Send(new DeleteProblemReportCommand { Id = id }, ct);
        }

    }
}


