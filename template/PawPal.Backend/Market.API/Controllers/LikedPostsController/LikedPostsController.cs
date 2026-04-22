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
using PawPal.Application.Modules.Posts.Queries.ListPostByRange;
using PawPal.Application.Modules.LikedUserPosts.Commands.Create;
using PawPal.Application.Modules.LikedUserPosts.Queries.GeyByID;
using PawPal.Application.Modules.LikedUserPosts.Queries.List;
using PawPal.Application.Modules.LikedUserPosts.Commands.Delete;
namespace PawPal.API.Controllers.Posts
{
    [ApiController]
    [Route("[controller]")]
    public class LikedPostsController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult<int>> CreateLikedPost(CreateLikedUserPostCommand command, CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]

        public async Task<GetLikedUserPostByQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await sender.Send(new GetLikedUserPostByIdQuery { UserId = id }, cancellationToken);
            return post;
        }
        [AllowAnonymous]
        [HttpGet]

        public async Task<ListLikedUserPostDto> GetLikedPostList([FromQuery] ListLikedUserPost query, CancellationToken cancellationToken)
        {
            var list = await sender.Send(query, cancellationToken);
            return list;
        }
        [AllowAnonymous]
        [HttpDelete("{id:int}")]

        public async Task Delete(DeleteLikedUserPostCommand deletePost, CancellationToken ct)
        {
            await sender.Send(new DeleteLikedUserPostCommand { UserId = deletePost.UserId,PostId = deletePost.PostId }, ct);
        }
    }
}
