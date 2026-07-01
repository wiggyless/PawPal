using Microsoft.AspNetCore.SignalR;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Create;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.GetById;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;
using PawPal.Application.Modules.Comments.Commands.Create;
using PawPal.Application.Modules.Comments.Commands.Delete;
using PawPal.Application.Modules.Comments.Queries.List;
using PawPal.Infrastructure.Signal;

namespace PawPal.API.Controllers.Comments
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController(ISender sender) : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("post")]
        public async Task<ActionResult<int>> CreateComment
           (CreateCommentCommand command, CancellationToken ct)
        {
            int id = await sender.Send(command, ct);
            return id;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListCommentsQueryDto>> List([FromQuery] ListCommentsQuery query, CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task<Unit> Delete(int id, CancellationToken ct)
        {
            var result = await sender.Send(new DeleteCommentCommand { CommentID = id}, ct);
            return result;
        }
    }
}
