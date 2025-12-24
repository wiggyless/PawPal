using PawPal.Domain.Entities.Adoptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Create
{
    public sealed class CreateAdoptionRequestCommandHandler(IAppDbContext context) : IRequestHandler<CreateAdoptionRequestCommand,int>
    {
        public async Task<int> Handle(CreateAdoptionRequestCommand request,CancellationToken cancellationToken)
        {
            var user = context.Users.Where(x => x.Id == request.UserID).FirstOrDefaultAsync(cancellationToken);
            var post = context.Posts.Where(x=>x.Id == request.PostID).FirstOrDefaultAsync(cancellationToken);
            var req = context.AdoptionRequirements.Where(x => x.Id == request.RequirementID).FirstOrDefaultAsync(cancellationToken);
            if (user is null) throw new PawPalNotFoundException("User does not exist");
            else if (post is null) throw new PawPalNotFoundException("Post does not exist");
            else if (req is null) throw new PawPalNotFoundException("Adoption requirement does not exist");
            var newRequest = new AdoptionRequestEntity
            {
                UserId = request.UserID,
                PostId = request.PostID,
                RequirementId = request.RequirementID,
                DateSent = DateTime.Now,
                Status = "Sent",
            };
            context.AdoptionRequests.Add(newRequest);
            await context.SaveChangesAsync(cancellationToken);
            return newRequest.Id;
        }
    }
}
