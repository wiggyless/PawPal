using PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create;
using PawPal.Application.Modules.Moderation.ReportedPosts.Commands.Delete;
using PawPal.Application.Modules.Moderation.ReportedPosts.Queries.List;
using PawPal.Application.Modules.Moderation.ReportedUsers.Commands.Delete;
using PawPal.Application.Modules.Moderation.ReportedUsers.Queries.GetById;
using PawPal.Application.Modules.Places.Cantons.Lists;
using PawPal.Application.Modules.Places.Cantons.Queries_;
using PawPal.Application.Modules.Posts.Queries.GetByID;
using PawPal.Application.Modules.Users.Commands.Create;

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
        [HttpGet("{id:int}")]

        public async Task<GetPostByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await sender.Send(new GetPostByIdQuery { Id = id }, cancellationToken);
            return post;
        }
        [HttpPost]
        public async Task<ActionResult<int>> CreateReportedPost(CreateReportPostCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return id;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Unit>> DeleteReportedPost(int id, CancellationToken ct)
        {
            return await sender.Send(new DeleteReportedPostCommand { Id = id }, ct);
        }
    }
}
