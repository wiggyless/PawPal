using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Allergies.Queries.GetById
{
    public sealed class GetAllergyByIdHandler(IAppDbContext context) : IRequestHandler<GetAllergyById,GetAllergyByIdDto>
    {
        public async Task<GetAllergyByIdDto> Handle(GetAllergyById request,CancellationToken cancellationToken)
        {
            var allergy = await context.Allergies.Where(x => x.Id == request.Id)
                .Select(x => new GetAllergyByIdDto
                {
                    AllergyID = x.Id,
                    Description = x.Description,
                    Name = x.Name,
                }).FirstOrDefaultAsync(cancellationToken);
            if (allergy is null)
                throw new PawPalNotFoundException("Allergy does not exist");
            return allergy;
        }
    }
}
