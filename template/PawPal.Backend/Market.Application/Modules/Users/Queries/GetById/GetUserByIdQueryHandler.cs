using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Users.Queries.GetById
{
    public sealed class GetUserByIdQueryHandler(IAppDbContext context)
        : IRequestHandler<GetUserByIdQuery,GetUserByIdQueryDto>
    {
        public async Task<GetUserByIdQueryDto> Handle(GetUserByIdQuery request,CancellationToken cancellationToken)
        {
            var user = await context.Users.
                Where(a => a.Id == request.Id).
                Select(x => new GetUserByIdQueryDto
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    DateTime = x.BirthDate,
                }).FirstOrDefaultAsync(cancellationToken);
            if (user == null) throw new PawPalNotFoundException($"User with Id {request.Id} does not exist");
            return user;
        }
    }
}
