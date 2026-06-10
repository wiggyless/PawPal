using FirebaseAdmin.Messaging;

namespace PawPal.Application.Services;

public class FirebaseNotificationService
{
    public async Task SendAsync(string fcmToken, string title, string body, string redirectUrl)
    {
        var message = new Message
        {
            Token = fcmToken,
            Notification = new Notification
            {
                Title = title,
                Body = body
            },
            Data = new Dictionary<string, string>
            {
                { "redirectUrl", redirectUrl }
            }
        };

        await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
}