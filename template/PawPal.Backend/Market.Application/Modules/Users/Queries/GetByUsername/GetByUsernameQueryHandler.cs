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
        public async Task<GetByUsernameQueryDto> Handle(GetByUsernameQuery request, CancellationToken cancellationToken)
        {
            var username = request.Username;

            var exists = await context.Users.AnyAsync(u => u.Username == username, cancellationToken);

            return new GetByUsernameQueryDto
            {
                Username = username,
                Exists = exists
            };
        }
    }
    
}
