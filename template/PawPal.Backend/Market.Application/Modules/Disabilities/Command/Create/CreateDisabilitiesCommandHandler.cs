using PawPal.Domain.Entities.Animal_Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Disabilities.Command.Create
{
    public sealed class CreateDisabilitiesCommandHandler(IAppDbContext context,IAppCurrentUser currentUser) : IRequestHandler<CreateDisabilitieCommand,int>
    {
        public async Task<int> Handle(CreateDisabilitieCommand command,CancellationToken cancellationToken)
        {
            if(!currentUser.IsAuthenticated || currentUser.RoleId !=3)
            {
                throw new PawPalNotFoundException("User is not allowed to do this action");
            }
            var newDisabilite = new DisabilitiesEntity
            {
                Name = command.Name,
                Description = command.Description
            };
             context.Disabilities.Add(newDisabilite);
            await context.SaveChangesAsync(cancellationToken);
            return newDisabilite.Id;
        }
    }
}
