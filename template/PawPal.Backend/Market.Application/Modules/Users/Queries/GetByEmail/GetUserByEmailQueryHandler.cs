using PawPal.Application.Modules.Users.Queries.GetByUsername;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetByEmail
{
    public sealed class GetUserByEmailQueryHandler(IAppDbContext context)
       : IRequestHandler<GetUserByEmailQuery, GetUserByEmailQueryDto>
    {
        public Task<GetUserByEmailQueryDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var email = request.Email;

            var user = context.Users
                .Where(u => u.Email == email)
                .Select(u => new GetUserByEmailQueryDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                var nullUser = new GetUserByEmailQueryDto
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
