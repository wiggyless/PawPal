using PawPal.Application.Modules.Users.Queries.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.ListDisabledAccounts
{
    public class ListDisabledAccountsQueryHandler(IAppDbContext context,IAppCurrentUser currentUser)
        : IRequestHandler<ListDisabledAccountsQuery, PageResult<ListDisabledAccountsQueryDto>>
    {
        public async Task<PageResult<ListDisabledAccountsQueryDto>> Handle(ListDisabledAccountsQuery request, CancellationToken cancellationToken)
        {
            if(!currentUser.IsAuthenticated || currentUser.RoleId !=3)
            {
                throw new PawPalConflictException("User is not allowed to do this action");
            }
            var usr = context.Users.Include(x => x.Role).Where(x=>x.isUserDisabled == true).AsNoTracking().AsQueryable();
            var userImages = context.UserImage.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchFirstName))
                usr = usr.Where(x => x.FirstName.ToLower().Contains(request.SearchFirstName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchLastName))
                usr = usr.Where(x => x.LastName.ToLower().Contains(request.SearchLastName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchEmail))
                usr = usr.Where(x => x.Email.ToLower().Contains(request.SearchEmail.ToLower()));
            usr = usr.Where(x => x.Role.RoleName != "Admin");
            var result = usr
            .GroupJoin(
                userImages,
                u => u.Id,
                ui => ui.UserID,
                (u, imgs) => new { u, imgs })
                .SelectMany(
                x => x.imgs.DefaultIfEmpty(),
                (x, img) => new ListDisabledAccountsQueryDto
                {
                    Id = x.u.Id,
                    FirstName = x.u.FirstName,
                    LastName = x.u.LastName,
                    Email = x.u.Email,
                    Username = x.u.Username,
                    PhotoURL = img != null ? img.PhotoURL : null
                });
            var finalResult = result.OrderBy(x => x.FirstName);
            return await PageResult<ListDisabledAccountsQueryDto>.FromQueryableAsync(finalResult, request.Paging, cancellationToken);
        }
    }
}
