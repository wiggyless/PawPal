using PawPal.Application.Modules.Allergies.Queries.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Disabilities.GetById
{
    public sealed class GetDisabilityByIdHandler(IAppDbContext context) : IRequestHandler<GetDisabilityById, GetDisabilityByIdDto>
    {
        public async Task<GetDisabilityByIdDto> Handle(GetDisabilityById request, CancellationToken cancellationToken)
        {
            var disability = await context.Disabilities.Where(x => x.Id == request.Id)
                .Select(x => new GetDisabilityByIdDto
                {
                    DisabilityID = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                }).FirstOrDefaultAsync(cancellationToken);
            if (disability is null)
                throw new PawPalNotFoundException("Disability does not exist");
            return disability;
        }
    }
}
