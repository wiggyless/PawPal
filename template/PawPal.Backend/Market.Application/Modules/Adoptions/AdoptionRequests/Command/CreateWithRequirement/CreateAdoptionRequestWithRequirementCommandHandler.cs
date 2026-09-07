using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Posts;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.CreateWithRequirement
{
    public sealed class CreateAdoptionRequestWithRequirementCommandHandler(
        IAppDbContext context,
        IFirebaseNotificationService firebaseNotificationService,
        IAppCurrentUser currentUser)
        : IRequestHandler<CreateAdoptionRequestWithRequirementCommand, int>
    {
        public async Task<int> Handle(CreateAdoptionRequestWithRequirementCommand request, CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
                throw new PawPalConflictException("User is not authenticated to do this action");

            if (request.PeopleCount < 0)
                throw new PawPalConflictException("Invalid number of people");

            var user = await context.Users
                .FirstOrDefaultAsync(x => x.Id == currentUser.UserId, cancellationToken);
            if (user is null) throw new PawPalNotFoundException("User does not exist");

            var post = await context.Posts
                .FirstOrDefaultAsync(x => x.Id == request.PostID, cancellationToken);
            if (post is null) throw new PawPalNotFoundException("Post does not exist");

            if (post.UserId == user.Id)
                throw new PawPalConflictException("The same user cannot request to its own post");

            if (post.Status != PostStatus.Active)
                throw new PawPalConflictException("This animal is no longer available for adoption");

            var existing = await context.AdoptionRequests
                .Where(x => x.PostId == request.PostID
                    && x.UserId == user.Id
                    && x.Status == AdoptionRequestStatus.Pending)
                .FirstOrDefaultAsync(cancellationToken);
            if (existing is not null)
                throw new PawPalConflictException("You already have a pending request for this post");

            var requirement = new AdoptionRequirementEntity
            {
                CreatedByUserId = user.Id,
                HouseType = request.HouseType,
                Address = request.Address,
                ChildrenAround = request.ChildrenAround ?? false,
                ElderlyAround = request.ElderlyAround ?? false,
                OtherPetsAround = request.OtherPetsAround ?? false,
                YardAvailable = request.YardAvailable ?? false,
                PeopleCount = request.PeopleCount,
                FloorNumber = request.FloorNumber ?? 0,
                PeopleAva = request.PeopleAva ?? string.Empty,
                PlanedStay = request.PlanedStay ?? "Unknown",
                HouseDetials = request.HouseDetials ?? "No details provided",
                YardDetails = request.YardDetails ?? "Unknown",
                PetExp = request.PetExp ?? false,
                ExpDetails = request.ExpDetails ?? "Unknown",
                IsGift = request.IsGift ?? false,
                SumMoney = request.SumMoney ?? 0,
                Allergy = request.Allergy ?? false,
                Aggressiveness = request.Aggressiveness ?? false,
                TakeBack = request.TakeBack ?? false,
                FinalComment = request.FinalComment ?? "None",
            };

            var adoptionRequest = new AdoptionRequestEntity
            {
                UserId = user.Id,
                PostId = post.Id,
                Requirement = requirement,
                DateSent = DateTime.Now,
                Status = AdoptionRequestStatus.Pending,
            };

            await using var transaction = await context.BeginTransactionAsync(cancellationToken);
            try
            {
                context.AdoptionRequirements.Add(requirement);
                context.AdoptionRequests.Add(adoptionRequest);
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            var postOwner = await context.Users
                .FirstOrDefaultAsync(x => x.Id == post.UserId, cancellationToken);
            if (postOwner?.FcmToken is not null)
            {
                await firebaseNotificationService.SendAsync(
                    postOwner.FcmToken,
                    "New Adoption Request",
                    $"{user.Username} wants to adopt your animal!",
                    "/client/my-profile/my-requests"
                );
            }

            return adoptionRequest.Id;
        }
    }
}
