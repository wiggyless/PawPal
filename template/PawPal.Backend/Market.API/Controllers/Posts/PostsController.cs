using PawPal.Application.Modules.Posts.Commands.Create;
using PawPal.Application.Modules.Posts.Commands.Delete;
using PawPal.Application.Modules.Posts.Commands.Update;
using PawPal.Application.Modules.Posts.Queries.GetByID;
using PawPal.Application.Modules.Posts.Queries.List;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PawPal.Application.Modules.Posts.Queries.ListPostsByUserId;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.Animal_Info.AnimalHealthHistory.Commands.Delete_;
namespace PawPal.API.Controllers.Posts
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult<int>> CreatePost(CreatePostCommand command,CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]

        public async Task<GetPostByIdQueryDto> GetById(int id,CancellationToken cancellationToken)
        {
            var post = await sender.Send(new GetPostByIdQuery { Id = id }, cancellationToken);
            return post;
        }
        [AllowAnonymous]
        [HttpGet("userPost")]

        public async Task<PageResult<ListPostByUserIdQueryDto>> GetPostListById([FromQuery] ListPostByUserIdQuery query, CancellationToken cancellationToken)
        {
            var list = await sender.Send(query, cancellationToken);
            return list;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListPostQueryDto>> List([FromQuery] ListPostQuery query,CancellationToken token)
        {
            var list = await sender.Send(query, token);
            return list;
        }
        
        [HttpPut("{id:int}")]
        public async Task Update(UpdatePostCommand upc, int id, CancellationToken ct)
        {
            upc.Id = id;
            await sender.Send(upc, ct);
        }
        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task Delete(DeletePostCommand deletePost, CancellationToken ct)
        {
            await sender.Send(deletePost, ct);
            await sender.Send(new DeletePostImageCommand { PostId = deletePost.Id }, ct);
        }
    }
}
