namespace PawPal.Application.Modules.PostImages.ListMainImages
{
    public class ListMainImageQuery : IRequest<List<ListMainImageQueryDto>>
    {
        public List<int> PostIds { get; set; } = new();
    }
}
