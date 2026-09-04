namespace PawPal.API.Controllers.Posts
{
    public class CreatePostImageRequest
    {
        [FromForm(Name = "postId")]
        public int PostId { get; set; }
        [FromForm(Name = "postImages")]
        public IFormFileCollection PostImages { get; set; }
    }
}
