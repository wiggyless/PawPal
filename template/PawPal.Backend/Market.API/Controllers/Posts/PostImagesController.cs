using PawPal.Application.Modules.PostImages.Commands.Create;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.PostImages.Commands.Update;
using PawPal.Application.Modules.PostImages.GetById;
using PawPal.Application.Modules.Posts.Commands.Create;
using PawPal.Application.Modules.Posts.Commands.Delete;
using PawPal.Application.Modules.Posts.Commands.Update;
using PawPal.Application.Modules.Posts.Queries.GetByID;

namespace PawPal.API.Controllers.Posts
{
    [ApiController]
    [Route("[controller]")]
    public class PostImagesController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult<int>> CreatePost(CreatePostImageCommand command, CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetPostImagesByIdDto> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await sender.Send(new GetPostImagesById { PostId = id }, cancellationToken);
            return post;
        }

        [HttpPut("{id:int}")]
        public async Task Update(UpdatePostImageCommand upc, int id, CancellationToken ct)
        {
            upc.PostId = id;
            await sender.Send(upc, ct);
        }
        [HttpDelete("{id:int}")]
        public async Task Delete(int id,List<string> postImages, CancellationToken ct)
        {
            await sender.Send(new DeletePostImageCommand { PostId = id,PostImages = postImages }, ct);
        }
    }
}
