namespace PawPal.API.Controllers.Posts
{
    public class UpdatePostImageRequest
    {
        [FromForm(Name = "postId")]
        public int PostId { get; set; }
        [FromForm(Name = "postImages")]
        public IFormFileCollection PostImages { get; set; }
    }
}
