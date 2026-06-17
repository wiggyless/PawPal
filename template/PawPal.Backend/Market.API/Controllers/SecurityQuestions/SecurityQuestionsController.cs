using Microsoft.AspNetCore.RateLimiting;
using PawPal.Application.Modules.PostImages.Commands.Delete;
using PawPal.Application.Modules.Posts.Commands.Create;
using PawPal.Application.Modules.Posts.Commands.Delete;
using PawPal.Application.Modules.Posts.Commands.Update;
using PawPal.Application.Modules.Posts.Queries.GetByID;
using PawPal.Application.Modules.Posts.Queries.List;
using PawPal.Application.Modules.Posts.Queries.ListPostByRange;
using PawPal.Application.Modules.Posts.Queries.ListPostsByUserId;
using PawPal.Application.Modules.Security.Questions.Commands.Create;
using PawPal.Application.Modules.Security.Questions.Commands.Delete;
using PawPal.Application.Modules.Security.Questions.Commands.Update;
using PawPal.Application.Modules.Security.Questions.Query.GetByEmail;
using PawPal.Application.Modules.Security.Questions.Query.List;

namespace PawPal.API.Controllers.SecurityQuestions
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityQuestionsController(ISender sender) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult<int>> CreateQuestion(CreateQuestionCommand command, CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            return id;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<PageResult<ListQuestionsQueryDto>> List([FromQuery] ListQuestionsQuery query, CancellationToken token)
        {
            var list = await sender.Send(query, token);
            return list;
        }
        [AllowAnonymous]
        [HttpGet("email")]
        public async Task<PageResult<GetSecurityQuestionByEmailQueryDto>> ListByEmail([FromQuery] GetSecurityQuestionByEmailQuery query, CancellationToken token)
        {
            var list = await sender.Send(query, token);
            return list;
        }

        [HttpPut("{id:int}")]
        public async Task Update(UpdateQuestionCommand upc, int id, CancellationToken ct)
        {
            upc.Id = id;
            await sender.Send(upc, ct);
        }
        [AllowAnonymous]
        [HttpDelete("{id:int}")]
        public async Task Delete(DeleteQuestionCommand deletePost, CancellationToken ct)
        {
            await sender.Send(deletePost, ct);
        }
    }
}
