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
            if (!string.IsNullOrWhiteSpace(request.SearchFirstName))
                usr = usr.Where(x => x.FirstName.ToLower().Contains(request.SearchFirstName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchLastName))
                usr = usr.Where(x => x.LastName.ToLower().Contains(request.SearchLastName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchEmail))
                usr = usr.Where(x => x.Email.ToLower().Contains(request.SearchEmail.ToLower()));
            usr = usr.Where(x => x.Role.RoleName != "Admin");
            var finalResult = usr.OrderBy(x => x.FirstName).Select(x => new ListUsersQueryDto
            {
                Id = x.Id,  
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Username = x.Username,
            });
            return await PageResult<ListUsersQueryDto>.FromQueryableAsync(finalResult, request.Paging, cancellationToken);
        }
    }
}
