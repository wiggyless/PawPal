using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PawPal.Application.Modules.UserImages.Queries.GetById
{
    public class GetUserImageByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetUserImageByIdQuery, IActionResult>
    {
        public async Task<IActionResult> Handle(GetUserImageByIdQuery query, CancellationToken cancellationToken)
        {
            var userImg = await context.UserImage.Where(x => x.UserID == query.UserID).FirstOrDefaultAsync();
            if (userImg == null) {
                throw new PawPalNotFoundException("User image does not exist");
            }
            var usrImg =  new GetUserImageByIdQueryDto
            {
                FileBytes = userImg.Data,
                ContentType = userImg.ContentType,
                FileName = userImg.Name
            };
            return new FileContentResult(usrImg.FileBytes, usrImg.ContentType)
            {
                FileDownloadName = usrImg.FileName
            };
        }
    }
}
