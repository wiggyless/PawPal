using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.List
{
    public sealed class ListUsersQueryHandler(IAppDbContext context)
        : IRequestHandler<ListUsersQuery,PageResult<ListUsersQueryDto>>
    {
        public async Task<PageResult<ListUsersQueryDto>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            
            var usr = context.Users.Include(x=>x.Role).AsNoTracking().AsQueryable();
            if (request.Disabled is not null)
            {
 
                if ((bool)request.Disabled)
                {
                    usr = usr.Where(x => x.isUserDisabled);
                }
                else
                {
                    usr = usr.Where(x => !x.isUserDisabled);
                }
               
            }else usr = usr.Where(x => !x.isUserDisabled);
            var userImages = context.UserImage.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchFirstName))
                usr = usr.Where(x => x.FirstName.ToLower().Contains(request.SearchFirstName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchLastName))
                usr = usr.Where(x => x.LastName.ToLower().Contains(request.SearchLastName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchEmail))
                usr = usr.Where(x => x.Email.ToLower().Contains(request.SearchEmail.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchUsername))
            {
                usr = usr.Where(x => x.Username.ToLower().Contains(request.SearchUsername.ToLower()));
            }
            usr = usr.Where(x => x.Role.RoleName != "Admin");
            var result = usr
            .GroupJoin(
                userImages,
                u => u.Id,
                ui => ui.UserID,
                (u, imgs) => new { u, imgs })
                .SelectMany(
                x => x.imgs.DefaultIfEmpty(),
                (x, img) => new ListUsersQueryDto
                {
                Id = x.u.Id,
                FirstName = x.u.FirstName,
                LastName = x.u.LastName,
                Email = x.u.Email,
                Username = x.u.Username,
                PhotoURL = img != null ? img.PhotoURL : null
                });
            var finalResult = result.OrderBy(x => x.FirstName);
            return await PageResult<ListUsersQueryDto>.FromQueryableAsync(finalResult, request.Paging, cancellationToken);
        }
    }
}
