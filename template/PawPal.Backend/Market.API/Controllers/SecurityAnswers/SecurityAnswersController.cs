using PawPal.Application.Modules.Security.Answers.Commands.Create;
using PawPal.Application.Modules.Security.Answers.Commands.Update;
using PawPal.Application.Modules.Security.Answers.Query.GetByQuestionAndEmail;
using PawPal.Application.Modules.Security.Questions.Commands.Create;
using PawPal.Application.Modules.Security.Questions.Commands.Delete;
using PawPal.Application.Modules.Security.Questions.Commands.Update;
using PawPal.Application.Modules.Security.Questions.Query.List;

namespace PawPal.API.Controllers.SecurityAnswers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityAnswersController(ISender sender) : ControllerBase
    {
        [HttpPost]

        public async Task<ActionResult<int>> CreateAnswer(CreateAnswerCommand command, CancellationToken cancellationToken)
        {
            int id = await sender.Send(command, cancellationToken);
            return id;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<GetAnswerQueryDto> List([FromQuery] GetAnswerQuery query, CancellationToken token)
        {
            var answer = await sender.Send(query, token);
            return answer;
        }

        [HttpPut]
        public async Task Update(UpdateAnswerCommand upc, CancellationToken ct)
        {
            await sender.Send(upc, ct);
        }
        [HttpDelete("{id:int}")]
        public async Task Delete(DeleteAnswerCommand deletePost, CancellationToken ct)
        {
            await sender.Send(deletePost, ct);
        }
    }
}
