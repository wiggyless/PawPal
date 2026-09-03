namespace PawPal.Application.Modules.PostImages.GetByIdFile
{
    public sealed class GetImagesPostByIdFileQueryHandler(IAppDbContext context, IFileStorageService fileStorage)
        : IRequestHandler<GetImagesPostByIdFileQuery, GetImagesPostByIdFileQueryDto>
    {
        public async Task<GetImagesPostByIdFileQueryDto> Handle(GetImagesPostByIdFileQuery request, CancellationToken cancellationToken)
        {
            var postImage = await context.PostImages.Where(x => x.PostId == request.PostId).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null)
                throw new PawPalNotFoundException("PostImages not found");

            var result = new GetImagesPostByIdFileQueryDto
            {
                PostId = request.PostId,
                PostImages = new List<byte[]>(),
            };

            foreach (var relativePath in postImage.PhotoURL)
            {
                result.PostImages.Add(await fileStorage.ReadFileAsync(relativePath, cancellationToken));
            }

            return result;
        }
    }
}
