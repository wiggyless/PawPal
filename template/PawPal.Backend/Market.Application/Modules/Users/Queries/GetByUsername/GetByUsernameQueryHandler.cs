using PawPal.Application.Modules.Users.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetByUsername
{
    public sealed class GetByUsernameQueryHandler(IAppDbContext context)
       : IRequestHandler<GetByUsernameQuery, GetByUsernameQueryDto>
    {
        public Task<GetByUsernameQueryDto> Handle(GetByUsernameQuery request, CancellationToken cancellationToken)
        {
            var username = request.Username;

            var user = context.Users
                .Where(u => u.Username == username)
                .Select(u => new GetByUsernameQueryDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username
                })
                .FirstOrDefaultAsync(cancellationToken);
            if(user == null)
            {
               var nullUser = new GetByUsernameQueryDto
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = string.Empty,
                    Username = string.Empty
                };
                return Task.FromResult(nullUser);
            }
            return user;
        }
    }
    
}
