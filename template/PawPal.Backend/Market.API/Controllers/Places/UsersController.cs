using PawPal.Application.Modules.Animal_Info.Animals.Commands.Delete;
using PawPal.Application.Modules.Users.Commands.Create;
using PawPal.Application.Modules.Users.Commands.Delete;
using PawPal.Application.Modules.Users.Commands.Update;
using PawPal.Application.Modules.Users.Queries.GetByEmail;
using PawPal.Application.Modules.Users.Queries.GetById;
using PawPal.Application.Modules.Users.Queries.GetByUsername;
using PawPal.Application.Modules.Users.Queries.List;

namespace PawPal.API.Controllers.Places
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(ISender sender):ControllerBase
    {

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<int>> CreateUser(CreateUserCommand cuc,CancellationToken ct)
        {
            int id = await sender.Send(cuc, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id});
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<GetUserByIdQueryDto> GetById(int id, CancellationToken ct)
        {
            var user = await sender.Send(new GetUserByIdQuery { Id = id }, ct);
            return user;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListUsersQueryDto>> List([FromQuery]
        ListUsersQuery query,CancellationToken ct)
        {
            var result = await sender.Send(query, ct);
            return result;
        }
        [AllowAnonymous]
        [HttpPut("{id:int}")]
        public async Task Update(UpdateUserCommand uuc, int id, CancellationToken ct)
        {
            uuc.Id = id;
            await sender.Send(uuc, ct);
        }

        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task Delete(int id, CancellationToken ct)
        {
            await sender.Send(new DeleteUserCommand { Id = id }, ct);
        }

        [AllowAnonymous]
        [HttpGet("lookup")] //to search by username or email
        public async Task<IActionResult> GetUser(
             [FromQuery] string? username,
             [FromQuery] string? email,
              CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var user = await sender.Send(new GetByUsernameQuery { Username = username }, ct);
                return Ok(user);
            }
            if (!string.IsNullOrEmpty(email))
            {
                var user = await sender.Send(new GetUserByEmailQuery { Email = email }, ct);
                return Ok(user);
            }
            return BadRequest("Provide either username or email.");
        }
    }
}
