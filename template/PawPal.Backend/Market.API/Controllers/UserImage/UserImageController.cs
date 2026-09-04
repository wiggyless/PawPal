using PawPal.API.Extensions;
using PawPal.Application.Modules.UserImages.Commands.Create;
using PawPal.Application.Modules.UserImages.Commands.Update;
using PawPal.Application.Modules.UserImages.Queries.GetById;
using PawPal.Application.Modules.UserImages.Queries.GetByIdFile;

namespace PawPal.API.Controllers.UserImage
{
    [ApiController]
    [Route("[controller]")]
    public class UserImageController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<int> CreatePost([FromForm] CreateUserImageRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateUserImageCommand { Image = request.Image.ToFileUpload() };
            return await sender.Send(command, cancellationToken);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetUserImageByIdQueryDto> GetById(int id, CancellationToken cancellationToken)
        {
            var user = await sender.Send(new GetUserImageByIdQuery { UserID = id }, cancellationToken);
            return user;
        }
        [AllowAnonymous]
        [HttpGet("download/{id:int}")]
        public async Task<GetUserImageByIdFileQueryDto> GetImageBlob(int id, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetUserImageByIdFileQuery { UserId = id }, cancellationToken);
        }

        [HttpPut]
        public async Task Update([FromForm] UpdateUserImageRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserImageCommand { Image = request.Image.ToFileUpload() };
            await sender.Send(command, cancellationToken);
        }
    }
}
