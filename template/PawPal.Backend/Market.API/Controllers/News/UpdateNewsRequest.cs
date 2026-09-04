namespace PawPal.API.Controllers.News
{
    public class UpdateNewsRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
