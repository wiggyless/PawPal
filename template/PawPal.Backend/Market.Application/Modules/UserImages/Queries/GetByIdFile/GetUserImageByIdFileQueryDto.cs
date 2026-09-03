namespace PawPal.Application.Modules.UserImages.Queries.GetByIdFile
{
    public class GetUserImageByIdFileQueryDto
    {
        public int UserId { get; set; }
        public byte[] Image { get; set; } = Array.Empty<byte>();
    }
}
