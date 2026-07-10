using PawPal.Application.Services;

namespace PawPal.Application.Modules.Adoptions.AdoptionRequests.Command.UpdateStatus;

public sealed class UpdateAdoptionStatusCommandHandler(
    IAppDbContext context,
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

        var validStatuses = new[] { "Accepted", "Denied" };
        if (!validStatuses.Contains(request.Status))
            throw new PawPalConflictException("Invalid status value");

        adoptionRequest.Status = request.Status;

        if (request.Status == "Accepted" && adoptionRequest.Post is not null)
        {
            adoptionRequest.Post.Status = "Adopted";
        }

        await context.SaveChangesAsync(cancellationToken);

        var requester = adoptionRequest.User;
        if (requester is null)
            return;

        if (request.Status == "Accepted")
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
                    // don't let a transient email failure fail the whole approval
                    // TODO: replace with proper logger once wired in
                }
            }
        }
        else if (request.Status == "Denied")
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
            <h2 style="color: #2e7d32;">Congratulations, {{requesterUsername}}! 🎉</h2>
            <p>Your adoption request for <strong>{{animalName}}</strong> has been approved by <strong>{{ownerUsername}}</strong>!</p>

            <p>Here are the next steps you should take:</p>
            <ol style="line-height: 1.8;">
                <li>Get in contact with <strong>{{ownerUsername}}</strong> and discuss a meetup location for the adoption handoff.</li>
                <li>Buy the supplies necessary for your new family member (food, bedding, toys, carrier/crate, etc.).</li>
                <li>Prepare your home so it's safe and comfortable before your new companion arrives.</li>
                <li>Schedule a check-up with a local veterinarian for the first few weeks.</li>
                <li>Be patient during the adjustment period — it can take time for your new pet to settle in.</li>
            </ol>

            <p>Thank you for choosing to adopt and giving an animal a loving home!</p>

            <p style="margin-top: 24px;">Warm regards,<br/><strong>PawPal Team</strong></p>
        </div>
        """;
    }
}