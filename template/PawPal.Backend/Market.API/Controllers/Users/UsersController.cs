using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Create;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Commands.Delete;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.GetById;
using PawPal.Application.Modules.Animal_Info.AnimalCategories.Queries_.List;
using PawPal.Application.Modules.Users.Queries.GetByUsername;

namespace PawPal.API.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(ISender sender) : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet("{username}")]
        public async Task<GetByUsernameQueryDto> GetById(string username, CancellationToken ct)
        {
            var user = await sender.Send(new GetByUsernameQuery { Username = username}, ct);
            return user;
        }

    }
}
