namespace PawPal.Application.Modules.PostImages.GetByIdFile
{
    public class GetImagesPostByIdFileQuery : IRequest<GetImagesPostByIdFileQueryDto>
    {
        public int PostId { get; set; }
    }
}
