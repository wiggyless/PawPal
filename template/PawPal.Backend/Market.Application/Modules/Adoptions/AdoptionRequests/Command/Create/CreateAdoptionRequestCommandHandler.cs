using PawPal.Application.Services;
using PawPal.Domain.Entities.Adoptions;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.Create
{
    public sealed class CreateAdoptionRequestCommandHandler(
        IAppDbContext context,
        IFirebaseNotificationService firebaseNotificationService,IAppCurrentUser currentUser)
        : IRequestHandler<CreateAdoptionRequestCommand, int>
    {
        public async Task<int> Handle(CreateAdoptionRequestCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
            {
                throw new PawPalConflictException("User is not authenticated to do this action");
            }
            var user = await context.Users.Where(x => x.Id == currentUser.UserId).FirstOrDefaultAsync(cancellationToken);
            var post = await context.Posts.Where(x => x.Id == request.PostID).FirstOrDefaultAsync(cancellationToken);
            var req = await context.AdoptionRequirements.Where(x => x.Id == request.RequirementID).FirstOrDefaultAsync(cancellationToken);
            if (user is null) throw new PawPalNotFoundException("User does not exist");
            if (post is null) throw new PawPalNotFoundException("Post does not exist");
            if (req is null) throw new PawPalNotFoundException("Adoption requirement does not exist");
            if (post.UserId == user.Id)
                throw new PawPalConflictException("The same user cannot request to its own post");

            var existing = await context.AdoptionRequests
                .Where(x => x.PostId == request.PostID && x.UserId == user.Id && x.Status == "Pending")
                .FirstOrDefaultAsync(cancellationToken);
            if (existing is not null)
                throw new PawPalConflictException("You already have a pending request for this post");

            var newRequest = new AdoptionRequestEntity
            {
                UserId = user.Id,
                PostId = request.PostID,
                RequirementId = request.RequirementID,
                DateSent = DateTime.Now,
                Status = "Pending",
            };
            context.AdoptionRequests.Add(newRequest);
            await context.SaveChangesAsync(cancellationToken);
            // notify the post owner
            var postOwner = await context.Users
                .Where(x => x.Id == post.UserId)
                .FirstOrDefaultAsync(cancellationToken);
             if (postOwner?.FcmToken is not null)
             {
                 await firebaseNotificationService.SendAsync(
                     postOwner.FcmToken,
                     "New Adoption Request",
                     $"{user.Username} wants to adopt your animal!",
                     $"/client/my-profile/my-requests"
                 );
             }
            return newRequest.Id;
        }
    }
}