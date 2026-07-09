using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Abstractions
{
    public interface IFirebaseNotificationService
    {
        Task SendAsync(string fcmToken, string title, string body, string redirectUrl);
    }
}
