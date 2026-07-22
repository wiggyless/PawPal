using PawPal.Application.Abstractions;

namespace PawPal.Application.Modules.Notifications.Commands.RegisterFcmToken;

public class RegisterFcmTokenCommandHandler(IAppDbContext db, IAppCurrentUser currentUser)
    : IRequestHandler<RegisterFcmTokenCommand>
{
    public async Task Handle(RegisterFcmTokenCommand request, CancellationToken ct)
    {
        var user = await db.Users.FirstOrDefaultAsync(x=>x.Id == currentUser.UserId, ct);
        if (user is null) return;

        user.FcmToken = request.Token;
        await db.SaveChangesAsync(ct);
    }
}