using Microsoft.AspNetCore.SignalR;
using PawPal.Application.Common;
using PawPal.Domain.Common;
using PawPal.Infrastructure.Signal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Infrastructure.Common
{
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
