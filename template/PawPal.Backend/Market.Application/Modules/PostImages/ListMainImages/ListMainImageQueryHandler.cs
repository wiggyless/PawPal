namespace PawPal.Application.Modules.PostImages.ListMainImages
{
    public sealed class ListMainImageQueryHandler(IAppDbContext context, IFileStorageService fileStorage)
        : IRequestHandler<ListMainImageQuery, List<ListMainImageQueryDto>>
    {
        public async Task<List<ListMainImageQueryDto>> Handle(ListMainImageQuery request, CancellationToken cancellationToken)
        {
            var postImages = await context.PostImages
                .Where(x => request.PostIds.Contains(x.PostId))
                .ToListAsync(cancellationToken);

            var result = new List<ListMainImageQueryDto>();
            foreach (var img in postImages)
            {
                result.Add(new ListMainImageQueryDto
                {
                    PostID = img.PostId,
                    MainImage = await fileStorage.ReadFileAsync(img.MainImage, cancellationToken),
                });
            }
            return result;
        }
    }
}
