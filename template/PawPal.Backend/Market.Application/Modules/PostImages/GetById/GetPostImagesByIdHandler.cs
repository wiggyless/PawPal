using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.PostImages.GetById
{
    public sealed class GetPostImagesByIdHandler(IAppDbContext context) : IRequestHandler<GetPostImagesById,GetPostImagesByIdDto>
    {
        public async Task<GetPostImagesByIdDto> Handle(GetPostImagesById request, CancellationToken cancellationToken)
        {
            var postImage = await context.PostImages.Where(x => x.PostId == request.PostId).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null)
                throw new PawPalNotFoundException("PostImages not found");
            var newPostImage = new GetPostImagesByIdDto
            {
                Id = postImage.Id,
                PostId = request.PostId,
                PostImages = postImage.PhotoURL,
            };
            return newPostImage;
        }
    }
}
