using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PawPal.Domain.Entities.Adoptions;
namespace PawPal.Application.Modules.Adoptions.AdoptionRequirements.Commands.Create
{
    public sealed class CreateRequirementCommandHandler(IAppDbContext context) : IRequestHandler<CreateRequirementCommand,int>
    {
        public async Task<int> Handle(CreateRequirementCommand command,CancellationToken cancellationToken)
        {
            if (command.PeopleCount < 0) throw new PawPalConflictException("Invalid number of people");
            var newRequirement = new AdoptionRequirementEntity
            {
                HouseType = command.HouseType,
                PeopleCount = command.PeopleCount,
                ChildrenAround = command.ChildrenAround,
                OtherPetsAround = command.OtherPetsAround,
                YardAvailable = command.YardAvailable,
            };
            context.AdoptionRequirements.Add(newRequirement);
            await context.SaveChangesAsync(cancellationToken);
            return newRequirement.Id;
        }
    }
}
