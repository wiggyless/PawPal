using PawPal.Application.Modules.Dashboard.Queries.GetSummary;

namespace PawPal.API.Controllers.Dashboard
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController(ISender sender) : ControllerBase
    {
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        [HttpGet("summary")]
        public async Task<GetDashboardSummaryQueryDto> GetSummary(CancellationToken cancellationToken)
        {
            return await sender.Send(new GetDashboardSummaryQuery(), cancellationToken);
        }
    }
}
