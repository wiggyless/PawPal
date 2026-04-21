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
                Address = command.Address,
                ChildrenAround = command.ChildrenAround ?? false,
                ElderlyAround = command.ElderlyAround ?? false,
                OtherPetsAround = command.OtherPetsAround ?? false,
                YardAvailable = command.YardAvailable ?? false,
                PeopleCount = command.PeopleCount,
                FloorNumber = command.FloorNumber ?? 0,
                PeopleAva = command.PeopleAva ?? string.Empty,
                PlanedStay = command.PlanedStay ?? "Unknown",
                HouseDetials = command.HouseDetials ?? "No details provided",
                YardDetails = command.YardDetails ?? "Unknown",
                PetExp = command.PetExp ?? false,
                ExpDetails = command.ExpDetails ?? "Unknown",
                IsGift = command.IsGift ?? false,
                SumMoney = command.SumMoney ?? 0,
                Allergy = command.Allergy ?? false,
                Aggressiveness = command.Aggressiveness ?? false,
                TakeBack = command.TakeBack ?? false,
                FinalComment = command.FinalComment ?? "None",
            };
            context.AdoptionRequirements.Add(newRequirement);
            await context.SaveChangesAsync(cancellationToken);
            return newRequirement.Id;
        }
    }
}
