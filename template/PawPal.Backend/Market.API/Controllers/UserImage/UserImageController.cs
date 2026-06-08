using PawPal.Application.Abstractions;
using PawPal.Application.Modules.UserImages.Commands.Create;
using PawPal.Application.Modules.UserImages.Commands.Update;
using PawPal.Application.Modules.UserImages.Queries.GetById;

namespace PawPal.API.Controllers.UserImage
{
    [ApiController]
    [Route("[controller]")]
    public class UserImageController(ISender sender, IAppDbContext context) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<int> CreateImage(CreateUserImageCommand command,CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            return id;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserImageById(int id,CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetUserImageByIdQuery { UserID = id}, cancellationToken);
            return result; 
        }
        [AllowAnonymous]
        [HttpPut]

        public async Task<int> UpdateUserImage(UpdateUserImageCommand query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return result;
        }
    }
}
