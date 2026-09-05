using PawPal.API.Extensions;
using PawPal.Application.Modules.PostImages.Commands.Create;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.PostImages.Commands.Update;
using PawPal.Application.Modules.PostImages.GetById;
using PawPal.Application.Modules.PostImages.GetByIdFile;
using PawPal.Application.Modules.PostImages.ListMainImages;

namespace PawPal.API.Controllers.Posts
{

    [ApiController]
    [Route("[controller]")]
    public class PostImagesController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> CreatePost([FromForm] CreatePostImageRequest request, CancellationToken cancellationToken)
        {
            var command = new CreatePostImageCommand
            {
                PostId = request.PostId,
                PostImages = request.PostImages.ToFileUploads()
            };
            int id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetPostImagesByIdDto> GetById(int id, CancellationToken cancellationToken)
        {
            var post = await sender.Send(new GetPostImagesById { PostId = id }, cancellationToken);
            return post;
        }
        [AllowAnonymous]
        [HttpGet("download/{id:int}")]
        public async Task<GetImagesPostByIdFileQueryDto> GetImageBlob(int id, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetImagesPostByIdFileQuery { PostId = id }, cancellationToken);
        }

        [AllowAnonymous]
        [HttpGet("catalogImages")]
        public async Task<List<ListMainImageQueryDto>> GetMainImages([FromQuery(Name = "id")] List<int> request, CancellationToken cancellationToken)
        {
            return await sender.Send(new ListMainImageQuery { PostIds = request }, cancellationToken);
        }

        [HttpPut]
        public async Task Update(UpdatePostImageRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdatePostImageCommand
            {
                PostId = request.PostId,
                PostImages = request.PostImages.ToFileUploads()
            };
            await sender.Send(command, cancellationToken);
        }
        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeletePostImageCommand { PostId = id}, ct);
        }
    }
}
