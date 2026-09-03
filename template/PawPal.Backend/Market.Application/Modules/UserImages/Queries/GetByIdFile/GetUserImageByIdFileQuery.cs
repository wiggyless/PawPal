namespace PawPal.Application.Modules.UserImages.Queries.GetByIdFile
{
    public class GetUserImageByIdFileQuery : IRequest<GetUserImageByIdFileQueryDto>
    {
        public int UserId { get; set; }
    }
}
