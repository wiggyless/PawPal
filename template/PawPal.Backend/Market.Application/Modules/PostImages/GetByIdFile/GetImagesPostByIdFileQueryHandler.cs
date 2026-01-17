using PawPal.Application.Modules.PostImages.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;


namespace PawPal.Application.Modules.PostImages.GetByIdFile
{

    public sealed class GetImagesPostByIdFileQueryHandler(IAppDbContext context) : IRequestHandler<GetImagesPostByIdFileQuerycs,GetImagesPostByIdFileQueryDto>
    {

        public async Task<GetImagesPostByIdFileQueryDto> Handle(GetImagesPostByIdFileQuerycs request, CancellationToken cancellationToken)
        {
            
            var postImage = await context.PostImages.Where(x => x.PostId == request.PostId).FirstOrDefaultAsync(cancellationToken);
            if (postImage is null)
                throw new PawPalNotFoundException("PostImages not found");

            var newPostImage = new GetImagesPostByIdFileQueryDto
            {
                PostId = request.PostId,
            };

            foreach(var img in postImage.PhotoURL)
            {

                newPostImage.PostImages.Add(await System.IO.File.ReadAllBytesAsync(img));
            }
            return newPostImage;
        }

    }
}
