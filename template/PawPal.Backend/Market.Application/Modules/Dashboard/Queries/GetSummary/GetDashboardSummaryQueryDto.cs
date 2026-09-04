namespace PawPal.Application.Modules.Dashboard.Queries.GetSummary
{
    public class GetDashboardSummaryQueryDto
    {
        public int ActiveListings { get; set; }
        public int PendingAdoptionRequests { get; set; }
        public int ReportedPosts { get; set; }
        public int ReportedUsers { get; set; }
        public int ReportedComments { get; set; }
        public int ReportedProblems { get; set; }
    }
}
