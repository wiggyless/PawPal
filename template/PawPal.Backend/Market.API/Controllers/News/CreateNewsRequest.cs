namespace PawPal.API.Controllers.News
{
    public class CreateNewsRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IFormFile? Photo { get; set; }
    }
}
