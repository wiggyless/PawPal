using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PawPal.Application.Abstractions;
using PawPal.Domain.Common;
using PawPal.Infrastructure.Signal;

namespace PawPal.Infrastructure.Common
{
    [AllowAnonymous]
    public class CommentHubService : ICommentHubService
    {
        private readonly IHubContext<CommentHub> _hubContext;

        public CommentHubService(IHubContext<CommentHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendCommentNotification(CommentDto comment)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveComment", comment);
        }
    }
}
