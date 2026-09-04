using PawPal.Domain.Entities.Adoptions;
using PawPal.Domain.Entities.Posts;
using PawPal.Shared.Constants;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.UpdateStatus;

public sealed class UpdateAdoptionStatusCommandHandler(
    IAppDbContext context,
    IAppCurrentUser currentUser,
    IFirebaseNotificationService firebaseNotificationService,
    IEmailService emailService)
    : IRequestHandler<UpdateAdoptionStatusCommand>
{
    public async Task Handle(UpdateAdoptionStatusCommand request, CancellationToken cancellationToken)
    {
        var adoptionRequest = await context.AdoptionRequests
            .Include(x => x.User)
            .Include(x => x.Post)
                .ThenInclude(p => p.User)
            .Include(x => x.Post)
                .ThenInclude(p => p.Animal)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (adoptionRequest is null)
            throw new PawPalNotFoundException("Adoption request does not exist");

        if (adoptionRequest.Post?.UserId != currentUser.UserId && currentUser.RoleId != Roles.Admin)
            throw new PawPalConflictException("Only the post owner can accept or deny adoption requests.");

        if (!Enum.TryParse<AdoptionRequestStatus>(request.Status, ignoreCase: true, out var newStatus)
            || (newStatus != AdoptionRequestStatus.Accepted && newStatus != AdoptionRequestStatus.Denied))
            throw new PawPalConflictException("Invalid status value");

        if (adoptionRequest.Status != AdoptionRequestStatus.Pending)
            throw new PawPalConflictException("Only pending requests can be accepted or denied");

        if (newStatus == AdoptionRequestStatus.Accepted && adoptionRequest.Post is not null && adoptionRequest.Post.Status != PostStatus.Active)
            throw new PawPalConflictException("This animal is no longer available for adoption");

        adoptionRequest.Status = newStatus;

        if (newStatus == AdoptionRequestStatus.Accepted && adoptionRequest.Post is not null)
        {
            adoptionRequest.Post.Status = PostStatus.Adopted;

            var otherPendingRequests = await context.AdoptionRequests
                .Where(x => x.PostId == adoptionRequest.PostId
                    && x.Id != adoptionRequest.Id
                    && x.Status == AdoptionRequestStatus.Pending)
                .ToListAsync(cancellationToken);

            foreach (var other in otherPendingRequests)
                other.Status = AdoptionRequestStatus.Denied;
        }

        await context.SaveChangesAsync(cancellationToken);

        var requester = adoptionRequest.User;
        if (requester is null)
            return;

        if (newStatus == AdoptionRequestStatus.Accepted)
        {
            if (requester.FcmToken is not null)
            {
                await firebaseNotificationService.SendAsync(
                    requester.FcmToken,
                    "Adoption Approved!",
                    "You have successfully adopted an animal!",
                    "/client/my-profile/my-requests/history"
                );
            }

            if (!string.IsNullOrWhiteSpace(requester.Email))
            {
                var animalName = adoptionRequest.Post?.Animal?.Name ?? "your new companion";
                var ownerName = adoptionRequest.Post?.User?.Username ?? "the owner";
                var subject = "Your adoption request has been approved!";
                var body = BuildApprovalEmailBody(requester.Username, ownerName, animalName);

                try
                {
                    await emailService.SendEmailAsync(requester.Email, subject, body);
                }
                catch (Exception)
                {
                    // Don't let a transient email failure fail the whole approval.
                }
            }
        }
        else if (newStatus == AdoptionRequestStatus.Denied)
        {
            if (requester.FcmToken is not null)
            {
                await firebaseNotificationService.SendAsync(
                    requester.FcmToken,
                    "Adoption Request Update",
                    "Your adoption request has been denied.",
                    "/client/my-profile/my-requests/history"
                );
            }
        }
    }

    private static string BuildApprovalEmailBody(string requesterUsername, string ownerUsername, string animalName)
    {
        return $$"""
        <div style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; color: #333;">
            <h2 style="color: #2e7d32;">Congratulations, {{requesterUsername}}! ??</h2>
            <p>Your adoption request for <strong>{{animalName}}</strong> has been approved by <strong>{{ownerUsername}}</strong>!</p>

            <p>Here are the next steps you should take:</p>
            <ol style="line-height: 1.8;">
                <li>Get in contact with <strong>{{ownerUsername}}</strong> and discuss a meetup location for the adoption handoff.</li>
                <li>Buy the supplies necessary for your new family member (food, bedding, toys, carrier/crate, etc.).</li>
                <li>Prepare your home so it's safe and comfortable before your new companion arrives.</li>
                <li>Schedule a check-up with a local veterinarian for the first few weeks.</li>
                <li>Be patient during the adjustment period � it can take time for your new pet to settle in.</li>
            </ol>

            <p>Thank you for choosing to adopt and giving an animal a loving home!</p>

            <p style="margin-top: 24px;">Warm regards,<br/><strong>PawPal Team</strong></p>
        </div>
        """;
    }
}