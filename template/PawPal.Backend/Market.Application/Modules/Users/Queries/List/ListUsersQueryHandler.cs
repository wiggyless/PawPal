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
            var usr = context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchFirstName))
                usr = context.Users.Where(x => x.FirstName.ToLower().Contains(request.SearchFirstName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchLastName))
                usr = context.Users.Where(x => x.LastName.ToLower().Contains(request.SearchLastName.ToLower()));
            if (!string.IsNullOrWhiteSpace(request.SearchEmail))
                usr = context.Users.Where(x => x.Email.ToLower().Contains(request.SearchEmail.ToLower()));
            var finalResult = usr.OrderBy(x => x.FirstName).Select(x => new ListUsersQueryDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                BirthDate = x.BirthDate,
            });
            return await PageResult<ListUsersQueryDto>.FromQueryableAsync(finalResult, request.Paging, cancellationToken);
        }
    }
}
