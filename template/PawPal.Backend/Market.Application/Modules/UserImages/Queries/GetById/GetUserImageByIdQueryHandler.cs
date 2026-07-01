using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using PawPal.Application.Modules.PostImages.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PawPal.Application.Modules.UserImages.Queries.GetById
{
    public class GetUserImageByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetUserImageByIdQuery, GetUserImageByIdQueryDto>
    {
        public async Task<GetUserImageByIdQueryDto> Handle(GetUserImageByIdQuery query, CancellationToken cancellationToken)
        {
            var userimg = await context.UserImage.Where(x => x.UserID == query.UserID).FirstOrDefaultAsync(cancellationToken);
            if (userimg is null)
                throw new PawPalNotFoundException("PostImages not found");
            var newUserImage = new GetUserImageByIdQueryDto
            {
                Id = userimg.Id,
                UserID = query.UserID,
                PhotoURL = userimg.PhotoURL,
            };
            return newUserImage;
        }
    }
}
