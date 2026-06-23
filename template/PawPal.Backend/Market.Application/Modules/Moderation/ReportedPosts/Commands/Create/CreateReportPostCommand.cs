using PawPal.Domain.Entities.Moderation;

namespace PawPal.API.Controllers.Moderation.ReportedPosts.Commands.Create
{
    public class CreateReportPostCommand :IRequest<int>
    {
        public ReportPostEnum Reason { get; set; }
        public string? Description { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
    }
}
