namespace PawPal.Application.Modules.UserImages.Queries.GetByIdFile
{
    public sealed class GetUserImageByIdFileQueryHandler(IAppDbContext context, IFileStorageService fileStorage)
        : IRequestHandler<GetUserImageByIdFileQuery, GetUserImageByIdFileQueryDto>
    {
        public async Task<GetUserImageByIdFileQueryDto> Handle(GetUserImageByIdFileQuery request, CancellationToken cancellationToken)
        {
            var userImage = await context.UserImage.Where(x => x.UserID == request.UserId).FirstOrDefaultAsync(cancellationToken);
            if (userImage is null)
                throw new PawPalNotFoundException("UserImage not found");

            return new GetUserImageByIdFileQueryDto
            {
                UserId = request.UserId,
                Image = await fileStorage.ReadFileAsync(userImage.PhotoURL, cancellationToken),
            };
        }
    }
}
