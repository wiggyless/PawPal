using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using FcmMessage = FirebaseAdmin.Messaging.Message;
using FcmNotification = FirebaseAdmin.Messaging.Notification;

namespace PawPal.Application.Services;

public class FirebaseNotificationService(ILogger<FirebaseNotificationService> logger) : IFirebaseNotificationService
{
    public async Task SendAsync(string fcmToken, string title, string body, string redirectUrl)
    {
        if (FirebaseApp.DefaultInstance is null)
        {
            logger.LogWarning("Firebase is not configured; skipping push notification \"{Title}\".", title);
            return;
        }

        var message = new FcmMessage
        {
            Token = fcmToken,
            Notification = new FcmNotification
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