using PawPal.Application.Modules.Users.Queries.GetByUsername;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetByEmail
{
    public sealed class GetUserByEmailQueryHandler(IAppDbContext context,IAppCurrentUser currentUser)
       : IRequestHandler<GetUserByEmailQuery, GetUserByEmailQueryDto>
    {
        public async Task<GetUserByEmailQueryDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var email = request.Email;

            var exists = await context.Users.AnyAsync(u => u.Email == email && !u.isUserDisabled, cancellationToken);

            return new GetUserByEmailQueryDto
            {
                Email = email,
                Exists = exists
            };
        }
    }

}
