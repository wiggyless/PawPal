using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List;
using PawPal.Application.Modules.Places.Cantons.Lists;
using PawPal.Application.Modules.Places.Cantons.Queries_;
using PawPal.Application.Modules.Users.Commands.Create;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PawPal.API.Controllers.Moderation
{
    [ApiController]
    [Route("[controller]")]
    public class ReportedPostsController(ISender sender) : ControllerBase
    {

        [HttpGet]
        public async Task<PageResult<ListReportedPostsQueryDto>> List([FromQuery]
        ListReportedPostsQuery query, CancellationToken tk)
        {
            var result = await sender.Send(query, tk);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateReportedPost(CreateReportPostCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return id;
        }
    }
}
