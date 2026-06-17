using PawPal.Domain.Common;
using PawPal.Domain.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawPal.Application.Abstractions
{
    public interface ICommentHubService 
    {
        Task SendCommentNotification(CommentDto comment);
    }
  
}
