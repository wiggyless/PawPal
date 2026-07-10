using FirebaseAdmin.Messaging;
using FcmMessage = FirebaseAdmin.Messaging.Message;
using FcmNotification = FirebaseAdmin.Messaging.Notification;

namespace PawPal.Application.Services;

public class FirebaseNotificationService : IFirebaseNotificationService
{
    public async Task SendAsync(string fcmToken, string title, string body, string redirectUrl) 
    {
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