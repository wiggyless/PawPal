using PawPal.Application.Services;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.UpdateStatus;

public sealed class UpdateAdoptionStatusCommandHandler(
    IAppDbContext context,
    FirebaseNotificationService firebaseNotificationService)
    : IRequestHandler<UpdateAdoptionStatusCommand>
{
    public async Task Handle(UpdateAdoptionStatusCommand request, CancellationToken cancellationToken)
    {
        var adoptionRequest = await context.AdoptionRequests
            .Include(x => x.User)
            .Include(x => x.Post)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (adoptionRequest is null)
            throw new PawPalNotFoundException("Adoption request does not exist");

        var validStatuses = new[] { "Accepted", "Declined" };
        if (!validStatuses.Contains(request.Status))
            throw new PawPalConflictException("Invalid status value");

        adoptionRequest.Status = request.Status;
        await context.SaveChangesAsync(cancellationToken);

        if (adoptionRequest.User?.FcmToken is not null)
        {
            await firebaseNotificationService.SendAsync(
                adoptionRequest.User.FcmToken,
                "Adoption Request Updated",
                $"Your adoption request has been {request.Status.ToLower()}!",
                $"/client/my-profile/my-requests"
            );
        }
    }
}